namespace InventorySystem.Application.StockMovements.DTOs;

public class StockMovementDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;

    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;

    public Guid? DestinationWarehouseId { get; set; }
    public string? DestinationWarehouseName { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}