using Application.Exceptions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Get the repository for Product
            var repository = _unitOfWork.GetRepository<Product>();

            // Get the existing product from the repository
            var product = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            // Delete the product from the repository
            await repository.DeleteAsync(product, cancellationToken);

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
