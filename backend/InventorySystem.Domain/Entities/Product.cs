using InventorySystem.Domain.Entities.Common;
using InventorySystem.Domain.Enums;
using InventorySystem.Domain.Exceptions;
using InventorySystem.Domain.ValueObjects;

namespace InventorySystem.Domain.Entities;

public class Product : AuditableEntity, ISoftDeletable
{
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public decimal Cost { get; private set; }
    public int MinimumStock { get; private set; }
    public int MaximumStock { get; private set; }
    public string? ImageUrl { get; private set; }
    public ProductStatus Status { get; private set; }
    public bool IsDeleted { get; private set; }
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public Guid? SupplierId { get; private set; }
    public Supplier? Supplier { get; private set; }

    private readonly List<StockMovement> _movements = new();
    public IReadOnlyCollection<StockMovement> Movements => _movements.AsReadOnly();

    private Product() { }

    public static Product Create(
        string sku,
        string name,
        decimal price,
        decimal cost,
        int minimumStock,
        int maximumStock,
        Guid categoryId,
        Guid? supplierId = null,
        string? description = null,
        string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required", nameof(sku));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required", nameof(name));

        if (price <= 0)
            throw new InvalidPriceException(price);

        if (cost < 0)
            throw new ArgumentException("Cost cannot be negative", nameof(cost));

        if (minimumStock < 0)
            throw new ArgumentException("Minimum stock cannot be negative", nameof(minimumStock));

        if (maximumStock < minimumStock)
            throw new ArgumentException("Maximum stock must be greater than minimum", nameof(maximumStock));

        return new Product
        {
            Sku = sku.ToUpperInvariant().Trim(),
            Name = name.Trim(),
            Description = description?.Trim(),
            Price = price,
            Cost = cost,
            MinimumStock = minimumStock,
            MaximumStock = maximumStock,
            CategoryId = categoryId,
            SupplierId = supplierId,
            ImageUrl = imageUrl,
            Status = ProductStatus.Active
        };
    }

    public void UpdateInfo(string name, string? description, decimal price, decimal cost, int minimumStock, int maximumStock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required", nameof(name));

        if (price <= 0)
            throw new InvalidPriceException(price);

        if (cost < 0)
            throw new ArgumentException("Cost cannot be negative", nameof(cost));

        if (minimumStock < 0)
            throw new ArgumentException("Minimum stock cannot be negative", nameof(minimumStock));

        if (maximumStock < minimumStock)
            throw new ArgumentException("Maximum stock must be greater than minimum", nameof(maximumStock));

        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        Cost = cost;
        MinimumStock = minimumStock;
        MaximumStock = maximumStock;
    }

    public void UpdateCategory(Guid categoryId) => CategoryId = categoryId;
    public void UpdateSupplier(Guid? supplierId) => SupplierId = supplierId;
    public void UpdateImage(string? imageUrl) => ImageUrl = imageUrl;

    public void Activate() => Status = ProductStatus.Active;
    public void Deactivate() => Status = ProductStatus.Inactive;
    public void Discontinue() => Status = ProductStatus.Discontinued;

    public void SoftDelete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;

    public int GetCurrentStock()
    {
        return _movements
            .Where(m => !m.IsDeleted)
            .Sum(m => m.Type == MovementType.Entry ? m.Quantity : -m.Quantity);
    }

    public bool IsLowStock() => GetCurrentStock() <= MinimumStock;
    public bool IsOverStock() => GetCurrentStock() >= MaximumStock;
}