namespace Application.Common.Models.PaymentService;
public class PaymentGatewayRequest
{
    public string TransactionId { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string CVV { get; set; }
}
