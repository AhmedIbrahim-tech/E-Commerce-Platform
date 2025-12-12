using Application.Common.Bases;
using Application.Common.Errors;
using Application.Common.Settings;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IPaymobCashInBroker broker,
    IMemoryCache memoryCache,
    INotificationStore notificationStore,
    INotificationSender notificationSender,
    ICurrentUserService currentUserService,
    PaymobSettings paymobSettings,
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<PlaceOrderCommand, ApiResponse<PaymentProcessResponse>>
{
    public async Task<ApiResponse<PaymentProcessResponse>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetTableAsTracking()
            .Where(c => c.Id.Equals(request.OrderId))
            .Include(c => c.Customer)
            .Include(c => c.ShippingAddress)
            .Include(c => c.Payment)
            .Include(c => c.Delivery)
            .Include(c => c.OrderItems)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null) return new ApiResponse<PaymentProcessResponse>(OrderErrors.OrderNotFound());

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Discount quantity from stock
            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await unitOfWork.Products.GetTableAsTracking()
                        .Where(c => c.Id.Equals(item.ProductId))
                        .Include(c => c.Category)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        await unitOfWork.Products.UpdateAsync(product);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return new ApiResponse<PaymentProcessResponse>(ProductErrors.ProductNotFound());
                    }
                }
            }
            else
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<PaymentProcessResponse>(OrderErrors.EmptyCart());
            }

            if (order.Payment?.PaymentMethod == null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<PaymentProcessResponse>(OrderErrors.InvalidPaymentMethod());
            }

            if (order.Delivery != null && order.Delivery.DeliveryMethod != DeliveryMethod.PickupFromBranch && order.ShippingAddressId == null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<PaymentProcessResponse>(OrderErrors.InvalidShippingAddress());
            }

            if (order.Payment.PaymentMethod == PaymentMethod.CashOnDelivery)
            {
                order.Status = Status.Completed;
                if (order.Payment != null)
                    order.Payment.Status = Status.Pending;

                await unitOfWork.Orders.UpdateAsync(order);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return Success(new PaymentProcessResponse(order.Id, "Success", order.PaymentToken ?? ""));
            }

            // Process Paymob payment
            try
            {
                var shippingAddress = order.ShippingAddress;
                var customer = await dbContext.Customers
                    .FirstOrDefaultAsync(c => c.Id == order.CustomerId, cancellationToken);
                
                if (customer is null)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<PaymentProcessResponse>(CustomerErrors.CustomerNotFound());
                }

                var appUser = await userManager.FindByIdAsync(customer.AppUserId.ToString());
                if (appUser is null)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<PaymentProcessResponse>(CustomerErrors.CustomerNotFound());
                }

                var amountCents = (int)(order.TotalAmount * 100 ?? 0);

                var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCents);
                var orderResponse = await broker.CreateOrderAsync(orderRequest);

                var fullName = customer.FullName ?? appUser.FullName ?? "Guest User";
                var nameParts = fullName.Split(' ', 2);
                string firstName = nameParts.Length > 0 ? nameParts[0] : "Guest";
                string lastName = nameParts.Length > 1 ? nameParts[1] : "User";

                var billingData = new CashInBillingData(
                    firstName: firstName,
                    lastName: lastName,
                    phoneNumber: appUser.PhoneNumber ?? "N/A",
                    email: appUser.Email ?? "N/A",
                    country: "N/A",
                    state: shippingAddress!.State!,
                    city: shippingAddress.City!,
                    apartment: "N/A",
                    street: shippingAddress.Street!,
                    floor: "N/A",
                    building: "N/A",
                    shippingMethod: order.Delivery!.DeliveryMethod.ToString()!,
                    postalCode: "N/A");

                if (!int.TryParse(paymobSettings.IntegrationId, out int integrationId))
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<PaymentProcessResponse>(PaymentErrors.PaymentProviderError());
                }

                var paymentKeyRequest = new CashInPaymentKeyRequest
                (
                    integrationId: integrationId,
                    orderId: orderResponse.Id,
                    billingData: billingData,
                    amountCents: amountCents,
                    currency: "EGP",
                    lockOrderWhenPaid: true,
                    expiration: 3600
                );

                var paymentKeyResponse = await broker.RequestPaymentKeyAsync(paymentKeyRequest);

                var payment = new Payment
                {
                    OrderId = order.Id,
                    TransactionId = orderResponse.Id.ToString(),
                    TotalAmount = order.TotalAmount,
                    PaymentDate = DateTimeOffset.UtcNow.ToLocalTime(),
                    PaymentMethod = PaymentMethod.Paymob,
                    Status = Status.Pending
                };

                await unitOfWork.Payments.AddAsync(payment);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                order.Status = Status.Completed;
                order.PaymentToken = paymentKeyResponse.PaymentKey;

                await unitOfWork.Orders.UpdateAsync(order);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                string iframeUrl = GetPaymentIframeUrl(paymentKeyResponse.PaymentKey);
                if (iframeUrl == "PaymentTokenCannotBeNullOrEmpty" || iframeUrl == "PaymobIframeIDIsNotConfigured")
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<PaymentProcessResponse>(PaymentErrors.PaymentProviderError());
                }

                var currentUserId = currentUserService.GetUserId().ToString();
                var notification = NotificationFactory.OrderPlaced(currentUserId, order.Id.ToString());
                var result = await notificationStore.AddNotification(notification);
                if (result == "Success")
                    await notificationSender.SendToUserAsync(notification.ReceiverId!, notification.Message!);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return Success(new PaymentProcessResponse(order.Id, iframeUrl, order.PaymentToken!));
            }
            catch (Exception)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<PaymentProcessResponse>(PaymentErrors.PaymentFailed());
            }
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<PaymentProcessResponse>(OrderErrors.OrderTotalMismatch());
        }
    }

    private string GetPaymentIframeUrl(string paymentToken)
    {
        if (string.IsNullOrEmpty(paymentToken))
            return "PaymentTokenCannotBeNullOrEmpty";

        string iframeId = paymobSettings.IframeId;
        if (string.IsNullOrEmpty(iframeId))
            return "PaymobIframeIDIsNotConfigured";

        string iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentToken}";
        return iframeUrl;
    }
}

