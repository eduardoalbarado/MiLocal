using Application.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries;

public class GetUserByB2CUserIdQuery : IRequest<bool>
{
    public string B2CUserId { get; set; }

    public GetUserByB2CUserIdQuery(string b2cUserId)
    {
        B2CUserId = b2cUserId;
    }
}

public class GetUserByB2CUserIdQueryHandler : IRequestHandler<GetUserByB2CUserIdQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByB2CUserIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(GetUserByB2CUserIdQuery request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<User>();
        var userSpec = new UserByB2CUserIdSpec(request.B2CUserId);
        var existingUser = await userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);

        return existingUser != null;
    }
}
