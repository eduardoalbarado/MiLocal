using Application.Common.Models;

namespace Application.Interfaces;
public interface IUserContextService
{
    UserContext GetUserContext();
    void SetUserContext(UserContext userContext);
    Task<bool> UserExistsAsync(string userId);
}
