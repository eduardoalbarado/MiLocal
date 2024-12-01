namespace Application.Common.Models;
public class UserContext
{
    public UserContext(string userId, string userName, string email, DateTime? lastLogin = null)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
        LastLogin = lastLogin;
    }

    public string UserId { get; set; }
    public string UserName { get; set; }
    public DateTime? LastLogin { get; set; }
    public string Email { get; set; }
}
