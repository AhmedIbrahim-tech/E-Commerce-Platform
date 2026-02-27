using System.Text.Json;

namespace Application.Features.Payments.Commands.ServerCallback;

public class ServerCallbackCommandHandler(
    IUnitOfWork unitOfWork,
    PaymobSettings paymobSettings,
    IMemoryCache memoryCache,
    IEmailService emailService,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<ServerCallbackCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ServerCallbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = request.Payload;
            var receivedHmac = request.Hmac;
            string HMAC = paymobSettings.HMAC;

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
                var order = await unitOfWork.Orders.GetTableAsTracking()
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
                    OrderId = orderId,
                    TransactionId = callback.Id.ToString(),
                    TotalAmount = callback.AmountCents / 100.0m,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = PaymentMethod.Paymob,
                    Status = Status.Pending,
                };
                await unitOfWork.Payments.AddAsync(payment, cancellationToken);
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

            await unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await unitOfWork.Payments.UpdateAsync(payment, cancellationToken);
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

                var customerForEmail = await unitOfWork.Customers.GetTableNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == order.CustomerId, cancellationToken);
                if (customerForEmail != null)
                {
                    var appUserForEmail = await userManager.FindByIdAsync(customerForEmail.AppUserId.ToString());
                    if (appUserForEmail != null)
                    {
                        var orderConfirmationBody = BuildOrderConfirmationEmailBody(order);
                        var emailDto = new EmailDto
                        {
                            MailTo = appUserForEmail.Email!,
                            Subject = "Order Placed Confirmation",
                            Body = orderConfirmationBody
                        };
                        await emailService.SendEmailsAsync(emailDto, cancellationToken);
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

    private static string BuildOrderConfirmationEmailBody(Order order)
    {
        var paymentStatus = order.Payment?.Status.ToString() ?? "Unknown";
        var statusColor = GetPaymentStatusColor(paymentStatus);
        var customerName = order.Customer?.FullName ?? "Customer";
        var orderDate = order.OrderDate.ToString("MMMM dd, yyyy");
        var deliveryTime = order.Delivery?.DeliveryTime?.ToString("MMMM dd, yyyy") ?? "N/A";
        var deliveryMethod = order.Delivery?.DeliveryMethod.ToString() ?? "N/A";
        var orderCost = order.TotalAmount;
        var deliveryCost = order.Delivery?.Cost ?? 0;
        var totalAmount = orderCost + deliveryCost;

        return $@"
            <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <h2>Order Placed Successfully!</h2>
                    <p>Dear {customerName},</p>
                    <p>Your order #{order.Id} has been successfully placed. We're now preparing your items for delivery or pickup.</p>
                    <table style='border-collapse: collapse; width: 100%; max-width: 600px;'>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Order ID:</td>
                            <td style='padding: 8px;'>{order.Id}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Order Date:</td>
                            <td style='padding: 8px;'>{orderDate}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Payment Status:</td>
                            <td style='padding: 8px; color: {statusColor};'>{paymentStatus}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Delivery Method:</td>
                            <td style='padding: 8px;'>{deliveryMethod}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Estimated Delivery Date:</td>
                            <td style='padding: 8px;'>{deliveryTime}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Order Cost:</td>
                            <td style='padding: 8px;'>{orderCost:F2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Delivery Cost:</td>
                            <td style='padding: 8px;'>{deliveryCost:F2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Total Amount:</td>
                            <td style='padding: 8px;'>{totalAmount:F2}</td>
                        </tr>
                    </table>
                    <p>You'll receive another notification when your order ships or is ready for pickup. If you have any questions, feel free to contact our support team.</p>
                    <p>Thank you for shopping with us!</p>
                    <p>Best regards,<br/>The Tajerly Team</p>
                </body>
            </html>";
    }

    private static string GetPaymentStatusColor(string paymentStatus)
    {
        return paymentStatus switch
        {
            "Completed" => "green",
            "Received" => "green",
            "Failed" => "red",
            "Pending" => "orange",
            "Refunded" => "blue",
            _ => "gray"
        };
    }
}

