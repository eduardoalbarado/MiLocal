using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Enabled { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            // Get the repository for Product
            var repository = _unitOfWork.GetRepository<Product>();

            // Create a new product object and set its properties
            var product = new Product
            {
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
                Price = request.Price,
                Enabled = request.Enabled
            };

            // Add the new product to the repository
            await repository.AddAsync(product);

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            // Return the id of the new product
            return product.Id;
        }
    }
}
