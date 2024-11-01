using Application.Common.Models;
using Application.Features.Users.Queries;
using Application.Interfaces;
using MediatR;

namespace Application.Services;

public class UserContextService : IUserContextService
{
    private UserContext _userContext;
    private readonly IMediator _mediator;

    public UserContextService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public UserContext GetUserContext()
    {
        return _userContext;
    }

    public void SetUserContext(UserContext userContext)
    {
        _userContext = userContext;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        var query = new GetUserByB2CUserIdQuery(userId);
        return await _mediator.Send(query);
    }
}
