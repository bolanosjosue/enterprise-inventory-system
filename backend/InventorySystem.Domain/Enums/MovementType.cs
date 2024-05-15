namespace InventorySystem.Domain.Enums;

public enum MovementType
{
    Entry = 1,        // Compra/Entrada
    Exit = 2,         // Venta/Salida
    Transfer = 3,     // Transferencia entre bodegas
    Adjustment = 4    // Ajuste manual
}