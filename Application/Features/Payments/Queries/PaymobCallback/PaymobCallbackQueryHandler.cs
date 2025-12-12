using Application.Common.Bases;
using Application.Common.Settings;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Payments.Queries.PaymobCallback;

public class PaymobCallbackQueryHandler(PaymobSettings paymobSettings) : ApiResponseHandler(),
    IRequestHandler<PaymobCallbackQuery, PaymobCallbackResponse>

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
        string calculatedHmac = ComputeHmacSHA512(concatenated, paymobSettings.HMAC);

        if (!request.Hmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(new PaymobCallbackResponse(
                false,
                HtmlGenerator.GenerateSecurityHtml()
            ));
        }

        bool.TryParse(request.Success, out bool isSuccess);

        return Task.FromResult(new PaymobCallbackResponse(
            isSuccess,
            isSuccess ? HtmlGenerator.GenerateSuccessHtml() : HtmlGenerator.GenerateFailedHtml()
        ));
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
}

