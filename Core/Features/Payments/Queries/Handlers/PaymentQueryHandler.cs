using Core.Features.Payments.Queries.Models;
using Core.Features.Payments.Queries.Responses;

namespace Core.Features.Payments.Queries.Handlers
{
    public class PaymentQueryHandler : ApiResponseHandler,
        IRequestHandler<PaymobCallbackQuery, PaymobCallbackResponse>
    {
        #region Fields
        private readonly IPaymobService _paymobService;
        private readonly PaymobSettings _paymobSettings;
        #endregion

        #region Constructors
        public PaymentQueryHandler(
            IPaymobService paymobService,
            PaymobSettings paymobSettings) : base()
        {            _paymobService = paymobService;
            _paymobSettings = paymobSettings;
        }
        #endregion

        #region Handle Functions
        public Task<PaymobCallbackResponse> Handle(PaymobCallbackQuery request, CancellationToken cancellationToken)
        {
            string[] fields = new[]
            {
        request.AmountCents, request.CreatedAt, request.Currency, request.ErrorOccured,
        request.HasParentTransaction, request.Id, request.IntegrationId, request.Is3dSecure,
        request.IsAuth, request.IsCapture, request.IsRefunded, request.IsStandalonePayment,
        request.IsVoided, request.Order, request.Owner, request.Pending,
        request.SourceDataPan, request.SourceDataSubType, request.SourceDataType, request.Success
    };

            var concatenated = string.Concat(fields);

            string calculatedHmac = _paymobService.ComputeHmacSHA512(concatenated, _paymobSettings.HMAC);

            if (!request.Hmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(new PaymobCallbackResponse
                (
                    false,
                    HtmlGenerator.GenerateSecurityHtml()
                ));
            }

            bool.TryParse(request.Success, out bool isSuccess);

            return Task.FromResult(new PaymobCallbackResponse
            (
                isSuccess,
                isSuccess ? HtmlGenerator.GenerateSuccessHtml() : HtmlGenerator.GenerateFailedHtml()
            ));
        }

        #endregion
    }
}
