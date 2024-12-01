using Application.Common.Models.Responses;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Net;

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
            var cart = await repository.FirstOrDefaultAsync(cartSpec, cancellationToken);

            if (cart == null)
            {
                throw new NotFoundException("Cart", request.UserId);
            }

            var cartDto = _mapper.Map<CartDto>(cart);

            return Result<CartDto>.Success(cartDto);
        }
    }
}
