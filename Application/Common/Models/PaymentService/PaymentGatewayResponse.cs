namespace Application.Common.Models.PaymentService;
public class PaymentGatewayResponse
{
    public string TransactionId { get; set; }
    public decimal Amount { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}