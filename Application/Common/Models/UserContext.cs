namespace Application.Common.Models;
public class UserContext
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public DateTime? LastLogin { get; set; }
    // Additional properties related to the authenticated user

    public UserContext(string userId, string userName, DateTime? lastLogin = null)
    {
        UserId = userId;
        UserName = userName;
        LastLogin = lastLogin;
    }
}
