using Application.Common.Models.PaymentService;

namespace Application.Interfaces.PaymentService
{
    /// <summary>
    /// Represents a client for interacting with the Payment Gateway.
    /// </summary>
    public interface IPaymentGatewayClient
    {
        /// <summary>
        /// Sends a payment request to the Payment Gateway and retrieves the payment response.
        /// </summary>
        /// <param name="request">The payment request details.</param>
        /// <returns>The payment response from the Payment Gateway.</returns>
        Task<PaymentGatewayResponse> ProcessPaymentAsync(PaymentGatewayRequest request);

        /// <summary>
        /// Retrieves the transaction status from the Payment Gateway for a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>The transaction status from the Payment Gateway.</returns>
        Task<PaymentStatus> GetTransactionStatusAsync(string transactionId);

        /// <summary>
        /// Updates the transaction status in the Payment Gateway for a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="newStatus">The new transaction status.</param>
        /// <returns>The updated response from the Payment Gateway.</returns>
        Task<PaymentGatewayUpdateResponse> UpdateTransactionStatus(string transactionId, PaymentStatus newStatus);
    }
}
