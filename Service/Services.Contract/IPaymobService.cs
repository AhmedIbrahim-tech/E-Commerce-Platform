

namespace Service.Services.Contract
{
    public interface IPaymobService
    {
        string ComputeHmacSHA512(string data, string secret);
        string GetPaymentIframeUrl(string paymentToken);
        Task<(Order?, string)> ProcessPaymentForOrderAsync(Order order);
        Task<string> ProcessTransactionCallbackAsync(CustomCashInCallbackTransaction callback, Guid orderId);
    }
}
