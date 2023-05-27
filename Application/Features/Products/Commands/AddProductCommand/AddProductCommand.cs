using Domain.Entities;
using MediatR;
using AutoMapper;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommand : AddProductDto, IRequest<int>
    {
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Product>();
            var product = _mapper.Map<Product>(request);
            await repository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }
    }
}
