using InventorySystem.Application.Common.Interfaces;

namespace InventorySystem.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}