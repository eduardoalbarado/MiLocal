using Domain.ValueObjects;

namespace Application.Common.Models.Checkout;

public class StartCheckoutRequestDto
{
    public string ShippingMethod { get; set; }
    public string ShippingAddress { get; set; }
    public string PaymentMethod { get; set; }
    public Location Location { get; set; }
}
