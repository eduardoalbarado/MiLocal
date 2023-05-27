using Application.Common.Models;

namespace Application.Interfaces;
public interface IEmailService
{
    Task SendAsync(EmailMessageDto message);
}
