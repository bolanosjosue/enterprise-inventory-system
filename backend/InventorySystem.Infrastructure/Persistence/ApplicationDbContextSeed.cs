using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Categories
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                Category.Create("Electrónica", "Productos electrónicos y tecnología"),
                Category.Create("Ropa", "Vestimenta y accesorios"),
                Category.Create("Alimentos", "Productos alimenticios"),
                Category.Create("Hogar", "Artículos para el hogar"),
                Category.Create("Deportes", "Equipamiento deportivo")
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // Suppliers
        if (!await context.Suppliers.AnyAsync())
        {
            var suppliers = new List<Supplier>
            {
                Supplier.Create("Tech Solutions S.A.", "123456789", "tech@solutions.com", "1234-5678", "San José, Costa Rica"),
                Supplier.Create("Fashion Import", "987654321", "contact@fashion.com", "8765-4321", "Heredia, Costa Rica"),
                Supplier.Create("Food Distributors", "555666777", "info@food.com", "5555-6666", "Alajuela, Costa Rica")
            };

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }

        // Warehouses
        if (!await context.Warehouses.AnyAsync())
        {
            var warehouses = new List<Warehouse>
            {
                Warehouse.Create("Bodega Central", "San José, Barrio Aranjuez", 10000),
                Warehouse.Create("Bodega Norte", "Heredia, Centro", 5000)
            };

            await context.Warehouses.AddRangeAsync(warehouses);
            await context.SaveChangesAsync();
        }

        // Products
        if (!await context.Products.AnyAsync())
        {
            var electronica = await context.Categories.FirstAsync(c => c.Name == "Electrónica");
            var ropa = await context.Categories.FirstAsync(c => c.Name == "Ropa");
            var alimentos = await context.Categories.FirstAsync(c => c.Name == "Alimentos");
            var hogar = await context.Categories.FirstAsync(c => c.Name == "Hogar");
            var deportes = await context.Categories.FirstAsync(c => c.Name == "Deportes");

            var techSupplier = await context.Suppliers.FirstAsync(s => s.TaxId == "123456789");
            var fashionSupplier = await context.Suppliers.FirstAsync(s => s.TaxId == "987654321");
            var foodSupplier = await context.Suppliers.FirstAsync(s => s.TaxId == "555666777");

            var products = new List<Product>
            {
                Product.Create("LAPTOP-001", "Laptop Dell XPS 15", 1299.99m, 950.00m, 5, 50, electronica.Id, techSupplier.Id,
                    "Laptop de alto rendimiento con procesador Intel i7", null),
                Product.Create("PHONE-001", "iPhone 15 Pro", 999.99m, 750.00m, 10, 100, electronica.Id, techSupplier.Id,
                    "Smartphone de última generación", null),
                Product.Create("SHIRT-001", "Camisa Casual", 29.99m, 15.00m, 20, 200, ropa.Id, fashionSupplier.Id,
                    "Camisa de algodón 100%", null),
                Product.Create("JEAN-001", "Jeans Levi's", 59.99m, 35.00m, 15, 150, ropa.Id, fashionSupplier.Id,
                    "Pantalón de mezclilla clásico", null),
                Product.Create("RICE-001", "Arroz Premium 1kg", 2.99m, 1.50m, 100, 1000, alimentos.Id, foodSupplier.Id,
                    "Arroz de grano largo", null),
                Product.Create("BEANS-001", "Frijoles Negros 500g", 1.99m, 1.00m, 150, 1500, alimentos.Id, foodSupplier.Id,
                    "Frijoles de alta calidad", null),
                Product.Create("CHAIR-001", "Silla de Oficina", 149.99m, 90.00m, 10, 50, hogar.Id, techSupplier.Id,
                    "Silla ergonómica con soporte lumbar", null),
                Product.Create("LAMP-001", "Lámpara LED", 19.99m, 10.00m, 30, 200, hogar.Id, techSupplier.Id,
                    "Lámpara de escritorio ajustable", null),
                Product.Create("BALL-001", "Balón de Fútbol", 24.99m, 12.00m, 25, 150, deportes.Id, fashionSupplier.Id,
                    "Balón profesional tamaño 5", null),
                Product.Create("YOGA-001", "Tapete de Yoga", 34.99m, 18.00m, 20, 100, deportes.Id, fashionSupplier.Id,
                    "Tapete antideslizante 6mm", null)
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Users
        if (!await context.Users.AnyAsync())
        {
            var user = User.Create(
                "admin@inventory.com",
                BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                "Administrador Sistema",
                UserRole.Admin
            );

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        // Stock Movements
        if (!await context.StockMovements.AnyAsync())
        {
            var laptop = await context.Products.FirstAsync(p => p.Sku == "LAPTOP-001");
            var phone = await context.Products.FirstAsync(p => p.Sku == "PHONE-001");
            var shirt = await context.Products.FirstAsync(p => p.Sku == "SHIRT-001");
            var rice = await context.Products.FirstAsync(p => p.Sku == "RICE-001");
            var ball = await context.Products.FirstAsync(p => p.Sku == "BALL-001");

            var bodegaCentral = await context.Warehouses.FirstAsync(w => w.Name == "Bodega Central");
            var bodegaNorte = await context.Warehouses.FirstAsync(w => w.Name == "Bodega Norte");

            var movements = new List<StockMovement>
            {
                StockMovement.CreateEntry(laptop.Id, bodegaCentral.Id, 20, 950.00m, "PO-001", "Compra inicial"),
                StockMovement.CreateEntry(phone.Id, bodegaCentral.Id, 50, 750.00m, "PO-002", "Compra inicial"),
                StockMovement.CreateEntry(shirt.Id, bodegaNorte.Id, 100, 15.00m, "PO-003", "Compra inicial"),
                StockMovement.CreateEntry(rice.Id, bodegaCentral.Id, 500, 1.50m, "PO-004", "Compra inicial"),
                StockMovement.CreateEntry(ball.Id, bodegaNorte.Id, 50, 12.00m, "PO-005", "Compra inicial")
            };

            await context.StockMovements.AddRangeAsync(movements);
            await context.SaveChangesAsync();
        }
    }
}