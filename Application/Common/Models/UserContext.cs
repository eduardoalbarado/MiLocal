namespace Application.Common.Models;
public class UserContext
{
    public UserContext(string userId, string userName, DateTime? lastLogin = null)
    {
        UserId = userId;
        UserName = userName;
        LastLogin = lastLogin;
    }

    public string UserId { get; set; }
    public string UserName { get; set; }
    public DateTime? LastLogin { get; set; }

}
