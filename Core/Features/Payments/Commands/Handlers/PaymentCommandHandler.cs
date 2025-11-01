using Core.Features.Payments.Commands.Models;
using System.Text;
using System.Text.Json;

namespace Core.Features.Payments.Commands.Handlers
{
    public class PaymentCommandHandler : ApiResponseHandler,
        IRequestHandler<SetPaymentMethodCommand, ApiResponse<string>>,
        IRequestHandler<ServerCallbackCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IPaymobService _paymobService;
        private readonly PaymobSettings _paymobSettings;
        #endregion

        #region Constructors
        public PaymentCommandHandler(IOrderService orderService,
            IPaymobService paymobService,
            PaymobSettings paymobSettings) : base()
        {
            _orderService = orderService;
            _paymobService = paymobService;
            _paymobSettings = paymobSettings;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(SetPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != Status.Draft)
                return BadRequest<string>("InvalidOrder");

            if (order.Payment == null)
                order.Payment = new Payment();

            order.Payment.PaymentMethod = request.PaymentMethod;
            order.Payment.Status = Status.Draft;
            var result = await _orderService.EditOrderAsync(order);
            if (result != "Success")
                return BadRequest<string>("UpdateFailed");
            return Success("");
        }

        public async Task<ApiResponse<string>> Handle(ServerCallbackCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payload = request.Payload;
                var receivedHmac = request.Hmac;
                string HMAC = _paymobSettings.HMAC;

                if (!payload.TryGetProperty("obj", out var obj))
                    return BadRequest<string>("Missing 'obj' in payload.");

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

                string calculatedHmac = _paymobService.ComputeHmacSHA512(concatenated.ToString(), HMAC);

                if (!receivedHmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
                    return BadRequest<string>("InvalidHMAC");

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

                    if (callback is null) return NotFound<string>("InvalidCallbackData");
                    var orderId = Guid.Parse(callback.OrderId!);
                    var order = await _orderService.GetOrderByIdAsync(orderId);
                    if (order is null) return NotFound<string>("OrderNotFound");

                    string result = await _paymobService.ProcessTransactionCallbackAsync(callback, orderId);

                    return result switch
                    {
                        "PaymentNotFound" => NotFound<string>("PaymentNotFound"),
                        "FailedToUpdateOrder" => BadRequest<string>("FailedToUpdateOrder"),
                        "FailedToSendOrderConfirmationEmail" => BadRequest<string>("FailedToSendOrderConfirmationEmail"),
                        "FailedToDeleteCartAfterOrderSuccess" => BadRequest<string>("FailedToDeleteCartAfterOrderSuccess"),
                        _ => Success("")
                    };
                }
                return BadRequest<string>("PaymentIntentIdMissing");
            }
            catch (Exception)
            {
                return BadRequest<string>("ErrorProcessingServerCallback");
            }
        }
        #endregion
    }
}
