using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Queries.GetCartByUserId
{
    public class GetCartByUserIdQuery : IRequest<Result<CartDto>>
    {
        public Guid UserId { get; set; }
    }

    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, Result<CartDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCartByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CartDto>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Cart>();
            var cartSpec = new CartByUserIdSpecification(request.UserId);
            var cart = await repository.GetBySpecAsync(cartSpec, cancellationToken);

            if (cart == null)
            {
                return Result<CartDto>.Failure($"Cart for user with UserId {request.UserId} not found");
            }

            var cartDto = _mapper.Map<CartDto>(cart);

            return Result<CartDto>.Success(cartDto);
        }
    }
}
