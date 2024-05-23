using InventorySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Sku)
            .IsUnique();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Cost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.MinimumStock)
            .IsRequired();

        builder.Property(p => p.MaximumStock)
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .HasDefaultValue(new byte[] { 0 })
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.UpdatedAt);

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(100);

        // Relaciones
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Movements)
            .WithOne(m => m.Product)
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter para soft delete
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}