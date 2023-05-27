using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : UpdateProductDto, IRequest<Unit>
    {
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // Get the repository for Product
            var repository = _unitOfWork.GetRepository<Product>();

            // Get the existing product from the repository
            var product = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            // Update the properties of the existing product
            product.Name = request.Name;
            product.ShortName = request.ShortName;
            product.Description = request.Description;
            product.Price = request.Price;
            product.PriceInDollars = request.PriceInDollars;
            product.Cost = request.Cost;
            product.CostInDollars = request.CostInDollars;
            product.Enabled = request.Enabled;
            product.Kit = request.Kit;

            // Update the product in the repository
            repository.UpdateAsync(product, cancellationToken);

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
