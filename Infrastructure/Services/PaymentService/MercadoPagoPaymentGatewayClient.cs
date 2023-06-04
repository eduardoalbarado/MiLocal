using Application.Common.Models.PaymentService;
using Application.Interfaces.PaymentService;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Http;
using MercadoPago.Resource.Payment;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Services.PaymentService;

public class MercadoPagoPaymentGatewayClient : IPaymentGatewayClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MercadoPagoPaymentGatewayClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    private void ConfigureMercadoPago()
    {
        var accessToken = _configuration.GetValue<string>("MercadoPago:AccessToken");
        var retryStrategy = new DefaultRetryStrategy(2);
        MercadoPagoConfig.AccessToken = accessToken;
        MercadoPagoConfig.RetryStrategy = retryStrategy;
        }

    public async Task<PaymentGatewayResponse> ProcessPaymentAsync(PaymentGatewayRequest request)
    {
        ConfigureMercadoPago();

        var createPaymentRequest = CreatePaymentCreateRequest(request);

        var paymentClient = new PaymentClient();
        Payment payment = await paymentClient.CreateAsync(createPaymentRequest);

        var response = MapPaymentToResponse(payment);
        return response;
    }

    public async Task<PaymentStatus> GetTransactionStatusAsync(string paymentId)
    {
        ConfigureMercadoPago();

        var payment = await GetPaymentByIdAsync(paymentId);

        if (Enum.TryParse(payment.Status, out PaymentStatus status))
        {
            return status;
        }

        return PaymentStatus.Other;
    }

    private PaymentCreateRequest CreatePaymentCreateRequest(PaymentGatewayRequest request)
    {
        return new PaymentCreateRequest
        {
            TransactionAmount = request.Amount,
            Token = "CARD_TOKEN",
            Description = request.Description,
            Installments = 1,
            PaymentMethodId = "visa",
            Payer = new PaymentPayerRequest
            {
                Email = request.Email
            }
        };
    }

    private async Task<Payment> GetPaymentByIdAsync(string paymentId)
    {
        var accessToken = _configuration["MercadoPago:AccessToken"];
        var requestUri = $"https://api.mercadopago.com/v1/payments/{paymentId}?access_token={accessToken}";
        var response = await _httpClient.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var payment = JsonConvert.DeserializeObject<Payment>(content);
            return payment;
        }

        throw new Exception("Failed to retrieve payment information from MercadoPago API.");
    }

    private PaymentGatewayResponse MapPaymentToResponse(Payment payment)
    {
        return new PaymentGatewayResponse
        {
            TransactionId = payment.Id.ToString() ?? string.Empty,
            Amount = payment.TransactionAmount ?? 0,
            IsSuccess = true,
            Message = $"Status: {payment.Status} StatusDetail: {payment.StatusDetail}"
        };
    }

    public Task<PaymentGatewayUpdateResponse> UpdateTransactionStatus(string transactionId, PaymentStatus newStatus)
    {
        throw new NotImplementedException(); // TODO: Implement UpdateTransactionStatus to work with MercadoPago
    }
}
