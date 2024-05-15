namespace InventorySystem.Domain.Entities.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    void SoftDelete();
    void Restore();
}