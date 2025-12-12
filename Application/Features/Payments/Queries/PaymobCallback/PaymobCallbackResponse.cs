namespace Application.Features.Payments.Queries.PaymobCallback;

public record PaymobCallbackResponse
(
    bool IsSuccess,
    string HtmlContent
);

