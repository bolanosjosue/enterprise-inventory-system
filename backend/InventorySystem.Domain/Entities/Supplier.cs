using InventorySystem.Domain.Entities.Common;

namespace InventorySystem.Domain.Entities;

public class Supplier : AuditableEntity, ISoftDeletable
{
    public string Name { get; private set; } = string.Empty;
    public string TaxId { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Supplier() { }

    public static Supplier Create(string name, string taxId, string? email = null, string? phone = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Supplier name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(taxId))
            throw new ArgumentException("Tax ID is required", nameof(taxId));

        return new Supplier
        {
            Name = name.Trim(),
            TaxId = taxId.Trim(),
            Email = email?.Trim(),
            Phone = phone?.Trim(),
            Address = address?.Trim(),
            IsActive = true
        };
    }

    public void UpdateInfo(string name, string taxId, string? email, string? phone, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Supplier name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(taxId))
            throw new ArgumentException("Tax ID is required", nameof(taxId));

        Name = name.Trim();
        TaxId = taxId.Trim();
        Email = email?.Trim();
        Phone = phone?.Trim();
        Address = address?.Trim();
    }

    public void UpdateContactInfo(string name, string? email, string? phone, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Supplier name is required", nameof(name));

        Name = name.Trim();
        Email = email?.Trim();
        Phone = phone?.Trim();
        Address = address?.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void SoftDelete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}