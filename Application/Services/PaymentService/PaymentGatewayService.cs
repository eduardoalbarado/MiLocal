using Application.Common.Models.PaymentService;
using Application.Interfaces.PaymentService;
using Domain.Entities;

namespace Application.Services.PaymentService;

public class PaymentGatewayService : IPaymentGatewayService
{
    private readonly IPaymentGatewayClient _paymentGatewayClient;

    public PaymentGatewayService(IPaymentGatewayClient paymentGatewayClient)
    {
        _paymentGatewayClient = paymentGatewayClient;
    }

    public async Task<PaymentGatewayResponse> ProcessPayment(Order order)
    {
        // Convert the request object to a format compatible with the Payment Gateway client
        var paymentGatewayRequest = MapOrderToPaymentGatewayRequest(order);

        // Send the request to the Payment Gateway client and retrieve the response
        var paymentGatewayResponse = await _paymentGatewayClient.ProcessPaymentAsync(paymentGatewayRequest);

        // Convert the Payment Gateway response to the application response object
        return MapPaymentGatewayResponseToPaymentGatewayResponse(paymentGatewayResponse);
    }
    public async Task<PaymentStatus> GetTransactionStatus(string transactionId)
    {
        var paymentStatus = await _paymentGatewayClient.GetTransactionStatusAsync(transactionId);
        return paymentStatus;
    }

    public async Task<UpdateTransactionStatusResponse> UpdateTransactionStatus(string transactionId, PaymentStatus newStatus)
    {
        // Call the Payment Gateway client to update the transaction status
        var updateResponse = await _paymentGatewayClient.UpdateTransactionStatus(transactionId, newStatus);

        // Convert the Payment Gateway update response to the application response object
        return MapToUpdateTransactionStatusResponse(updateResponse);
    }

    // Helper method to map Order object to PaymentRequest
    private PaymentGatewayRequest MapOrderToPaymentGatewayRequest(Order order)
    {
        var paymentGatewayRequest = new PaymentGatewayRequest
        {
            TransactionId = order.OrderId.ToString(),
            Email = order.CustomerEmail,
            Description = "Payment for order",
            Amount = order.TotalAmount,
            Currency = "USD",
            // Set other properties as needed
        };

        return paymentGatewayRequest;
    }
    public static UpdateTransactionStatusResponse MapToUpdateTransactionStatusResponse(PaymentGatewayUpdateResponse paymentGatewayUpdateResponse)
    {
        // Perform mapping logic here to transform the Payment Gateway update response
        // to the UpdateTransactionStatusResponse object

        var updateTransactionStatusResponse = new UpdateTransactionStatusResponse
        {
            IsSuccess = paymentGatewayUpdateResponse.Success,
            Message = paymentGatewayUpdateResponse.Message
        };

        return updateTransactionStatusResponse;
    }
    private PaymentGatewayResponse MapPaymentGatewayResponseToPaymentGatewayResponse(PaymentGatewayResponse paymentGatewayResponse)
    {
        return new PaymentGatewayResponse
        {
            TransactionId = paymentGatewayResponse.TransactionId,
            Amount = paymentGatewayResponse.Amount,
            IsSuccess = paymentGatewayResponse.IsSuccess,
            Message = paymentGatewayResponse.Message
        };
    }
}
