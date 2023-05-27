using Application.Common.Models;
using Application.Interfaces;

namespace Application.Services;
public class UserContextService : IUserContextService
{
    private UserContext _userContext;

    public UserContext GetUserContext()
    {
        return _userContext;
    }

    public void SetUserContext(UserContext userContext)
    {
        _userContext = userContext;
    }
}
