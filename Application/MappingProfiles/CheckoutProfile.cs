using Application.Common.Models.Checkout;
using Application.Features.Checkout.Commands.StartCheckout;
using AutoMapper;

namespace Application.MappingProfiles;

public class CheckoutProfile : Profile
{
    public CheckoutProfile()
    {
        CreateMap<StartCheckoutRequestDto, StartCheckoutCommand>();
    }
}
