using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByB2CUserId;

public class GetUserByIdQuery : IRequest<bool>
{
    public string B2CUserId { get; set; }

    public GetUserByIdQuery(string b2cUserId)
    {
        B2CUserId = b2cUserId;
    }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<User>();
        var userSpec = new UserByIdSpec(request.B2CUserId);
        var existingUser = await userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);

        return existingUser != null;
    }
}
