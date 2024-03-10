using Application.Common.Models.Responses;
using Application.Exceptions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.UpdateStock;

public class UpdateStockCommand : UpdateStockDto, IRequest<Result<Unit>>
{
    public int ProductId { get; set; }
}

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Product>();

        var product = await repository.GetByIdAsync(request.ProductId);


        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        product.StockQuantity = request.NewStockQuantity;
        await repository.UpdateAsync(product, cancellationToken);

        await _unitOfWork.SaveChangesAsync();

        return Result<Unit>.Success(Unit.Value);
    }
}
