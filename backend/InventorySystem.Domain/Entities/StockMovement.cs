using InventorySystem.Domain.Entities.Common;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities;

public class StockMovement : AuditableEntity, ISoftDeletable
{
    public MovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalAmount => Quantity * UnitPrice;
    public string Reference { get; private set; } = string.Empty;
    public string? Notes { get; private set; }
    public bool IsDeleted { get; private set; }

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public Guid WarehouseId { get; private set; }
    public Warehouse Warehouse { get; private set; } = null!;

    public Guid? DestinationWarehouseId { get; private set; }
    public Warehouse? DestinationWarehouse { get; private set; }

    private StockMovement() { }

    public static StockMovement CreateEntry(
        Guid productId,
        Guid warehouseId,
        int quantity,
        decimal unitPrice,
        string reference,
        string? notes = null)
    {
        ValidateQuantity(quantity);
        ValidateUnitPrice(unitPrice);
        ValidateReference(reference);

        return new StockMovement
        {
            Type = MovementType.Entry,
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Reference = reference.Trim(),
            Notes = notes?.Trim()
        };
    }

    public static StockMovement CreateExit(
        Guid productId,
        Guid warehouseId,
        int quantity,
        decimal unitPrice,
        string reference,
        string? notes = null)
    {
        ValidateQuantity(quantity);
        ValidateUnitPrice(unitPrice);
        ValidateReference(reference);

        return new StockMovement
        {
            Type = MovementType.Exit,
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Reference = reference.Trim(),
            Notes = notes?.Trim()
        };
    }

    public static StockMovement CreateTransfer(
        Guid productId,
        Guid sourceWarehouseId,
        Guid destinationWarehouseId,
        int quantity,
        decimal unitPrice,
        string reference,
        string? notes = null)
    {
        ValidateQuantity(quantity);
        ValidateUnitPrice(unitPrice);
        ValidateReference(reference);

        if (sourceWarehouseId == destinationWarehouseId)
            throw new ArgumentException("Source and destination warehouses must be different");

        return new StockMovement
        {
            Type = MovementType.Transfer,
            ProductId = productId,
            WarehouseId = sourceWarehouseId,
            DestinationWarehouseId = destinationWarehouseId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Reference = reference.Trim(),
            Notes = notes?.Trim()
        };
    }

    public static StockMovement CreateAdjustment(
        Guid productId,
        Guid warehouseId,
        int quantity,
        decimal unitPrice,
        string reference,
        string notes)
    {
        ValidateQuantity(quantity);
        ValidateUnitPrice(unitPrice);
        ValidateReference(reference);

        if (string.IsNullOrWhiteSpace(notes))
            throw new ArgumentException("Notes are required for adjustments", nameof(notes));

        return new StockMovement
        {
            Type = MovementType.Adjustment,
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Reference = reference.Trim(),
            Notes = notes.Trim()
        };
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
    }

    private static void ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
    }

    private static void ValidateReference(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Reference is required", nameof(reference));
    }

    public void SoftDelete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}