using InventorySystem.Domain.Entities.Common;

namespace InventorySystem.Domain.Entities;

public class Warehouse : AuditableEntity, ISoftDeletable
{
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public int? MaxCapacity { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<StockMovement> _movements = new();
    public IReadOnlyCollection<StockMovement> Movements => _movements.AsReadOnly();

    private Warehouse() { }

    public static Warehouse Create(string name, string address, int? maxCapacity = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Warehouse name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));

        if (maxCapacity.HasValue && maxCapacity.Value <= 0)
            throw new ArgumentException("Max capacity must be positive", nameof(maxCapacity));

        return new Warehouse
        {
            Name = name.Trim(),
            Address = address.Trim(),
            MaxCapacity = maxCapacity,
            IsActive = true
        };
    }

    public void UpdateInfo(string name, string address, int? maxCapacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Warehouse name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));

        if (maxCapacity.HasValue && maxCapacity.Value <= 0)
            throw new ArgumentException("Max capacity must be positive", nameof(maxCapacity));

        Name = name.Trim();
        Address = address.Trim();
        MaxCapacity = maxCapacity;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void SoftDelete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}