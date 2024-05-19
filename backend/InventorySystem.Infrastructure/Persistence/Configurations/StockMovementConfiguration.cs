using InventorySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type)
            .IsRequired();

        builder.Property(m => m.Quantity)
            .IsRequired();

        builder.Property(m => m.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.Reference)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Notes)
            .HasMaxLength(500);

        builder.Property(m => m.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.UpdatedAt);

        builder.Property(m => m.UpdatedBy)
            .HasMaxLength(100);

        // Índices para consultas frecuentes
        builder.HasIndex(m => m.CreatedAt);
        builder.HasIndex(m => new { m.ProductId, m.WarehouseId });

        // Relación con bodega destino (para transferencias)
        builder.HasOne(m => m.DestinationWarehouse)
            .WithMany()
            .HasForeignKey(m => m.DestinationWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}