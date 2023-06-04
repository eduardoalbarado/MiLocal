using Application.Common.Models.PaymentService;
using Domain.Entities;

namespace Application.Interfaces.PaymentService
{
    /// <summary>
    /// Represents a service for interacting with the Payment Gateway.
    /// </summary>
    public interface IPaymentGatewayService
    {
        /// <summary>
        /// Processes the payment for the given order and retrieves the payment response from the Payment Gateway.
        /// </summary>
        /// <param name="order">The order for which the payment is being processed.</param>
        /// <returns>The payment response from the Payment Gateway.</returns>
        Task<PaymentGatewayResponse> ProcessPayment(Order order);

        /// <summary>
        /// Retrieves the transaction status from the Payment Gateway for a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>The transaction status from the Payment Gateway.</returns>
        Task<PaymentStatus> GetTransactionStatus(string transactionId);

        /// <summary>
        /// Updates the transaction status in the Payment Gateway for a given transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="newStatus">The new transaction status.</param>
        /// <returns>The updated response from the Payment Gateway.</returns>
        Task<UpdateTransactionStatusResponse> UpdateTransactionStatus(string transactionId, PaymentStatus newStatus);
    }
}
