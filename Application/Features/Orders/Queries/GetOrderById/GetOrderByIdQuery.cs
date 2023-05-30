using Application.Common.Models;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
{
    public int OrderId { get; set; }
}

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);

        var repository = _unitOfWork.GetRepository<Order>();
        var orderSpec = new OrderByIdSpecification(userId, request.OrderId);
        var order = await repository.GetBySpecAsync(orderSpec, cancellationToken);

        if (order == null)
        {
            return Result<OrderDto>.Failure($"Order with ID {request.OrderId} not found.");
        }

        var orderDto = _mapper.Map<OrderDto>(order);

        return Result<OrderDto>.Success(orderDto);
    }
}
