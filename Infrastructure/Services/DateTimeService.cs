using Application.Interfaces;

namespace Infrastructure.Service;
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
}
