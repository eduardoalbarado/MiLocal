using Application.Common.Models.Responses;
using Application.Exceptions;
using Application.Features.Carts.Queries.GetCartByUserId;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Commands.ClearCart;

public class ClearCartCommand : IRequest<Result<Unit>>
{
}

public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public ClearCartCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<Result<Unit>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);

        var repository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await repository.FirstOrDefaultAsync(cartSpec, cancellationToken);

        if (cart == null)
        {
            throw new NotFoundException("Cart", userId);
        }

        cart.Items.Clear();

        await _unitOfWork.SaveChangesAsync();

        return Result<Unit>.Success(Unit.Value);
    }
}
