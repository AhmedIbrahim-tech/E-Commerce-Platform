using Application.Common.Bases;
using Application.Common.Errors;
using Application.Common.Settings;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace Application.Features.Payments.Commands.ServerCallback;

public class ServerCallbackCommandHandler(
    IUnitOfWork unitOfWork,
    PaymobSettings paymobSettings,
    IMemoryCache memoryCache,
    IEmailService emailService,
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<ServerCallbackCommand, ApiResponse<string>>

    public async Task<ApiResponse<string>> Handle(ServerCallbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = request.Payload;
            var receivedHmac = request.Hmac;
            string HMAC = _paymobSettings.HMAC;

            if (!payload.TryGetProperty("obj", out var obj))
                return new ApiResponse<string>(PaymentErrors.InvalidPaymentToken());

            // build concatenated string for HMAC
            string[] fields = new[]
            {
                "amount_cents", "created_at", "currency", "error_occured", "has_parent_transaction",
                "id", "integration_id", "is_3d_secure", "is_auth", "is_capture", "is_refunded",
                "is_standalone_payment", "is_voided", "order.id", "owner", "pending",
                "source_data.pan", "source_data.sub_type", "source_data.type", "success"
            };

            var concatenated = new StringBuilder();
            foreach (var field in fields)
            {
                string[] parts = field.Split('.');
                JsonElement current = obj;
                bool found = true;
                foreach (var part in parts)
                {
                    if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var next))
                        current = next;
                    else
                    {
                        found = false;
                        break;
                    }
                }

                if (!found || current.ValueKind == JsonValueKind.Null)
                    concatenated.Append("");
                else if (current.ValueKind == JsonValueKind.True || current.ValueKind == JsonValueKind.False)
                    concatenated.Append(current.GetBoolean() ? "true" : "false");
                else
                    concatenated.Append(current.ToString());
            }

            string calculatedHmac = ComputeHmacSHA512(concatenated.ToString(), HMAC);

            if (!receivedHmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
                return new ApiResponse<string>(PaymentErrors.InvalidPaymentToken());

            // Extract necessary fields from the payload
            string? paymentIntentId = null;
            if (obj.TryGetProperty("id", out var transactionIdElement) &&
                transactionIdElement.ValueKind != JsonValueKind.Null)
            {
                paymentIntentId = transactionIdElement.ToString();
            }

            bool isSuccess = obj.TryGetProperty("success", out var successElement) && successElement.GetBoolean();

            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                var callback = JsonSerializer.Deserialize<CustomCashInCallbackTransaction>(obj.GetRawText());

                if (callback is null) return new ApiResponse<string>(PaymentErrors.PaymentNotFound());
                var orderId = Guid.Parse(callback.OrderId!);
                var order = await _unitOfWork.Orders.GetTableAsTracking()
                    .Where(c => c.Id.Equals(orderId))
                    .Include(c => c.Customer)
                    .Include(c => c.ShippingAddress)
                    .Include(c => c.Payment)
                    .Include(c => c.Delivery)
                    .FirstOrDefaultAsync(cancellationToken);
                if (order is null) return new ApiResponse<string>(OrderErrors.OrderNotFound());

                string result = await ProcessTransactionCallbackAsync(callback, orderId, cancellationToken);

                return result switch
                {
                    "PaymentNotFound" => new ApiResponse<string>(PaymentErrors.PaymentNotFound()),
                    "FailedToUpdateOrder" => new ApiResponse<string>(OrderErrors.InvalidOrderStatus()),
                    "FailedToSendOrderConfirmationEmail" => new ApiResponse<string>(EmailErrors.EmailSendFailed()),
                    "FailedToDeleteCartAfterOrderSuccess" => new ApiResponse<string>(CartErrors.CannotModifyCart()),
                    _ => Success("")
                };
            }
            return new ApiResponse<string>(PaymentErrors.InvalidPaymentToken());
        }
        catch (Exception)
        {
            return new ApiResponse<string>(PaymentErrors.PaymentProviderError());
        }
    }

    private string ComputeHmacSHA512(string data, string secret)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var dataBytes = Encoding.UTF8.GetBytes(data);

        using (var hmac = new HMACSHA512(keyBytes))
        {
            var hash = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    private async Task<string> ProcessTransactionCallbackAsync(CustomCashInCallbackTransaction callback, Guid orderId, CancellationToken cancellationToken)
    {
        var payment = await unitOfWork.Payments.GetPaymentByTransactionId(callback.Id!.ToString());
        if (payment is null)
        {
            payment = await unitOfWork.Payments.GetPaymentByOrderId(orderId);

            if (payment is null)
            {
                payment = new Payment
                {
                    Id = orderId,
                    TransactionId = callback.Id.ToString(),
                    TotalAmount = callback.AmountCents / 100.0m,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = PaymentMethod.Paymob,
                    Status = Status.Pending,
                };
                await unitOfWork.Payments.AddAsync(payment);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        return callback switch
        {
            { Success: true } => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Completed, Status.Completed, cancellationToken),
            { IsRefunded: true } => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Pending, Status.Refunded, cancellationToken),
            { IsVoided: true } => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Pending, Status.Voided, cancellationToken),
            _ => await UpdateOrderStatusAsync(callback.Id.ToString(), Status.Failed, Status.Failed, cancellationToken)
        };
    }

    private async Task<string> UpdateOrderStatusAsync(string paymentIntentId, Status orderStatus, Status paymentStatus, CancellationToken cancellationToken)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var payment = await unitOfWork.Payments.GetPaymentByTransactionId(paymentIntentId)
                         ?? await unitOfWork.Payments.GetPaymentByOrderId(Guid.Parse(paymentIntentId));

            if (payment is null) return "PaymentNotFound";

            var order = await unitOfWork.Orders.GetTableAsTracking()
                .Where(o => o.Id == payment.OrderId)
                .Include(o => o.Delivery)
                .FirstOrDefaultAsync(cancellationToken);

            if (order is null) return "OrderNotFound";

            order.Status = orderStatus;
            if (order.Delivery!.DeliveryMethod != DeliveryMethod.PickupFromBranch)
                order.Delivery!.Status = orderStatus;

            payment.Status = paymentStatus;
            payment.PaymentDate = DateTimeOffset.UtcNow.ToLocalTime();

            await unitOfWork.Orders.UpdateAsync(order);
            await unitOfWork.Payments.UpdateAsync(payment);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (orderStatus == Status.Completed)
            {
                try
                {
                    var cartKey = $"cart:{order.CustomerId}";
                    memoryCache.Remove(cartKey);
                }
                catch (Exception)
                {
                    return "FailedToDeleteCartAfterOrderSuccess";
                }

                var customerForEmail = await dbContext.Customers
                    .FirstOrDefaultAsync(c => c.Id == order.CustomerId, cancellationToken);
                if (customerForEmail != null)
                {
                    var appUserForEmail = await userManager.FindByIdAsync(customerForEmail.AppUserId.ToString());
                    if (appUserForEmail != null)
                    {
                        var result3 = await emailService.SendEmailAsync(appUserForEmail.Email!, null!, EmailType.OrderConfirmation, order);
                        if (result3 != "Success")
                        {
                            await unitOfWork.RollbackTransactionAsync(cancellationToken);
                            return "FailedToSendOrderConfirmationEmail";
                        }
                    }
                }
            }

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return "Success";
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return "FailedToUpdateOrder";
        }
    }
}

