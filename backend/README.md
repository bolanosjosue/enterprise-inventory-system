# 🏢 Enterprise Inventory Management System - Backend

Sistema de gestión de inventario empresarial desarrollado con **Clean Architecture**, **CQRS** y **.NET 9**.

---

## 🚀 **Características Principales**

### **Gestión Completa de Inventario**
- ✅ **Productos**: CRUD completo con SKU único, precios, stock mínimo/máximo, imágenes
- ✅ **Categorías**: Organización jerárquica de productos con contador de productos
- ✅ **Proveedores**: Gestión de información, contacto y direcciones
- ✅ **Bodegas**: Control multi-almacén con capacidad máxima configurable

### **Movimientos de Inventario**
- ✅ **Compras (Entry)**: Ingreso de mercancía con validación de proveedor y bodega
- ✅ **Ventas (Exit)**: Salida con validación de stock disponible en tiempo real
- ✅ **Transferencias**: Movimiento entre bodegas con transacciones atómicas
- ✅ **Ajustes**: Correcciones manuales con notas y auditoría completa
- ✅ **Historial Completo**: Seguimiento de todos los movimientos con filtros por fecha, tipo y bodega
- ✅ **Stock por Bodega**: Consulta de inventario actual por almacén

### **Seguridad y Autenticación**
- ✅ **JWT Authentication**: Tokens seguros con expiración configurable (8 horas)
- ✅ **Autorización por Roles**: Admin, Supervisor, Operator, Viewer con permisos granulares
- ✅ **BCrypt Password Hashing**: Almacenamiento seguro de contraseñas con salt
- ✅ **Rate Limiting**: Protección contra fuerza bruta (100 req/min por cliente)
- ✅ **Soft Delete**: Eliminación lógica para mantener auditoría e integridad referencial
- ✅ **CORS Configurado**: Soporte para frontend en diferentes dominios

### **Calidad y Mantenibilidad**
- ✅ **Clean Architecture**: Separación clara de responsabilidades en 4 capas
- ✅ **CQRS con MediatR**: Comandos y consultas separados para mejor escalabilidad
- ✅ **Domain-Driven Design**: Entidades ricas con lógica de negocio encapsulada
- ✅ **FluentValidation**: Validaciones declarativas y reutilizables con mensajes claros
- ✅ **Exception Handling**: Middleware global con responses HTTP consistentes
- ✅ **Optimistic Concurrency**: Control de concurrencia con RowVersion en entidades críticas
- ✅ **Auditoría Automática**: CreatedBy, UpdatedBy, CreatedAt, UpdatedAt en todas las entidades

---

## 🛠️ **Stack Tecnológico**

### **Backend**
- **.NET 9.0** - Framework principal
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 9** - ORM con migraciones
- **PostgreSQL 14+** - Base de datos relacional
- **MediatR 12.x** - Implementación de CQRS pattern
- **FluentValidation 11.x** - Validaciones declarativas
- **BCrypt.Net 0.1.0** - Hashing de contraseñas
- **System.IdentityModel.Tokens.Jwt** - Generación y validación de JWT
- **Swashbuckle 6.x (Swagger)** - Documentación OpenAPI

### **Patrones y Principios**
> Nota: Los patrones aplicados se utilizan de forma pragmática,
> únicamente donde aportan claridad, mantenibilidad o escalabilidad.

#### **Clean Architecture (Onion Architecture)**
Separación en 4 capas independientes con dependencias hacia adentro:
- **Domain** (núcleo): Entidades, Value Objects, Enums
- **Application**: Casos de uso (Commands/Queries), DTOs, Interfaces
- **Infrastructure**: EF Core, Repositories, JWT, Servicios externos
- **API**: Controllers, Middleware, Configuración

#### **CQRS (Command Query Responsibility Segregation)**
Separación clara entre operaciones de escritura y lectura:
- **Commands**: `CreateProductCommand`, `UpdateProductCommand`, `ProcessSaleCommand`
- **Queries**: `GetProductsQuery`, `GetStockByWarehouseQuery`
- Validación con `FluentValidation` en Commands
- Handlers procesados por `MediatR`

#### **Domain-Driven Design (DDD)**
Entidades ricas con lógica de negocio encapsulada:
- **Aggregate Roots**: `Product`, `StockMovement`, `User`
- **Value Objects**: `Money` (para manejo de moneda)
- **Domain Events**: (preparado para implementar)
- **Factory Methods**: `Product.Create()`, `User.Create()`
- **Business Logic**: `Product.IsLowStock()`, `StockMovement.ValidateStock()`

#### **Repository Pattern**
Abstracción del acceso a datos:
- **Generic Repository**: `IRepository<T>` con operaciones CRUD base
- **Specific Queries**: Métodos especializados en handlers
- **Tracking Control**: `.AsNoTracking()` para queries de solo lectura
- **Include**: Eager loading de relaciones cuando es necesario

#### **Unit of Work Pattern**
Coordinación de transacciones:
- **IUnitOfWork**: Gestiona `SaveChangesAsync()` de forma centralizada
- **Atomic Operations**: Transferencias de stock como transacción única
- **Rollback Automático**: En caso de error, no se persisten cambios parciales

#### **Dependency Injection**
Inversión de control con .NET DI nativo:
- **Scoped Services**: Repositorios, DbContext
- **Transient Services**: Validators, Handlers
- **Singleton Services**: Configuraciones, Caches
- **Registration**: Centralizado en `DependencyInjection.cs`

#### **SOLID Principles**
- **S** (Single Responsibility): Cada handler tiene una única responsabilidad
- **O** (Open/Closed): Extensible mediante nuevos handlers sin modificar existentes
- **L** (Liskov Substitution): Interfaces respetadas (`IRepository<T>`)
- **I** (Interface Segregation): Interfaces pequeñas y específicas
- **D** (Dependency Inversion): Dependencias en abstracciones, no implementaciones

#### **Specification Pattern**
Queries reutilizables y composables:
- Filtros: `search`, `categoryId`, `status`
- Ordenamiento: Por nombre, fecha, precio
- Paginación: `page`, `pageSize`
- Ejemplo: `GetProductsQuery` con múltiples criterios combinables

#### **Otros Patrones Implementados**
- **Builder Pattern**: En configuraciones de EF Core
- **Factory Pattern**: Métodos estáticos `Create()` en entidades
- **Strategy Pattern**: Diferentes tipos de movimientos (Purchase, Sale, Transfer)
- **Interceptor Pattern**: `AuditableEntityInterceptor`, `SoftDeleteInterceptor`
- **Middleware Pipeline**: Manejo de excepciones y rate limiting
- **Result Pattern**: `Result<T>` para manejo de errores sin excepciones

---

## 📦 **Estructura del Proyecto**
```
backend/
│
├── InventorySystem.API/
│   ├── Controllers/
│   │   ├── ApiController.cs
│   │   ├── AuthController.cs
│   │   ├── CategoriesController.cs
│   │   ├── ProductsController.cs
│   │   ├── StockMovementsController.cs
│   │   ├── SuppliersController.cs
│   │   ├── UsersController.cs
│   │   └── WarehousesController.cs
│   │
│   ├── Extensions/
│   │   └── MiddlewareExtensions.cs
│   │
│   ├── Middleware/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   └── RateLimitingMiddleware.cs
│   │
│   ├── Properties/
│   │   └── launchSettings.json
│   │
│   ├── appsettings.json
│   ├── InventorySystem.API.csproj
│   ├── InventorySystem.API.http
│   └── Program.cs
│
├── InventorySystem.Application/
│   ├── Authentication/
│   │   ├── Commands/
│   │   │   ├── Login/
│   │   │   │   ├── LoginCommand.cs
│   │   │   │   ├── LoginCommandHandler.cs
│   │   │   │   └── LoginCommandValidator.cs
│   │   │   ├── Register/
│   │   │   │   ├── RegisterCommand.cs
│   │   │   │   ├── RegisterCommandHandler.cs
│   │   │   │   └── RegisterCommandValidator.cs
│   │   │   ├── ToggleUserStatus/
│   │   │   │   ├── ToggleUserStatusCommand.cs
│   │   │   │   └── ToggleUserStatusCommandHandler.cs
│   │   │   └── UpdateUserRole/
│   │   │       ├── UpdateUserRoleCommand.cs
│   │   │       ├── UpdateUserRoleCommandHandler.cs
│   │   │       └── UpdateUserRoleCommandValidator.cs
│   │   │
│   │   ├── DTOs/
│   │   │   ├── LoginDto.cs
│   │   │   └── UserDto.cs
│   │   │
│   │   ├── Queries/
│   │   │   └── GetUsers/
│   │   │       ├── GetUsersQuery.cs
│   │   │       └── GetUsersQueryHandler.cs
│   │   │
│   │   └── Services/
│   │       └── ITokenService.cs
│   │
│   ├── Categories/
│   │   ├── Commands/
│   │   │   ├── CreateCategory/
│   │   │   │   ├── CreateCategoryCommand.cs
│   │   │   │   ├── CreateCategoryCommandHandler.cs
│   │   │   │   └── CreateCategoryCommandValidator.cs
│   │   │   ├── DeleteCategory/
│   │   │   │   ├── DeleteCategoryCommand.cs
│   │   │   │   └── DeleteCategoryCommandHandler.cs
│   │   │   └── UpdateCategory/
│   │   │       ├── UpdateCategoryCommand.cs
│   │   │       ├── UpdateCategoryCommandHandler.cs
│   │   │       └── UpdateCategoryCommandValidator.cs
│   │   │
│   │   ├── DTOs/
│   │   │   └── CategoryDto.cs
│   │   │
│   │   └── Queries/
│   │       ├── GetCategories/
│   │       │   ├── GetCategoriesQuery.cs
│   │       │   └── GetCategoriesQueryHandler.cs
│   │       └── GetCategoryById/
│   │           ├── GetCategoryByIdQuery.cs
│   │           └── GetCategoryByIdQueryHandler.cs
│   │
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   ├── LoggingBehavior.cs
│   │   │   └── ValidationBehavior.cs
│   │   │
│   │   ├── Interfaces/
│   │   │   ├── IApplicationDbContext.cs
│   │   │   ├── ICurrentUserService.cs
│   │   │   ├── IDateTime.cs
│   │   │   ├── IRepository.cs
│   │   │   └── IUnitOfWork.cs
│   │   │
│   │   └── Models/
│   │       ├── Error.cs
│   │       ├── PaginatedList.cs
│   │       └── Result.cs
│   │
│   ├── Products/
│   │   ├── Commands/
│   │   │   ├── CreateProduct/
│   │   │   │   ├── CreateProductCommand.cs
│   │   │   │   ├── CreateProductCommandHandler.cs
│   │   │   │   └── CreateProductCommandValidator.cs
│   │   │   ├── DeleteProduct/
│   │   │   │   ├── DeleteProductCommand.cs
│   │   │   │   └── DeleteProductCommandHandler.cs
│   │   │   └── UpdateProduct/
│   │   │       ├── UpdateProductCommand.cs
│   │   │       ├── UpdateProductCommandHandler.cs
│   │   │       └── UpdateProductCommandValidator.cs
│   │   │
│   │   ├── DTOs/
│   │   │   └── ProductDto.cs
│   │   │
│   │   └── Queries/
│   │       ├── GetProductById/
│   │       │   ├── GetProductByIdQuery.cs
│   │       │   └── GetProductByIdQueryHandler.cs
│   │       └── GetProducts/
│   │           ├── GetProductsQuery.cs
│   │           └── GetProductsQueryHandler.cs
│   │
│   ├── StockMovements/
│   │   ├── Commands/
│   │   │   ├── ProcessPurchase/
│   │   │   │   ├── ProcessPurchaseCommand.cs
│   │   │   │   ├── ProcessPurchaseCommandHandler.cs
│   │   │   │   └── ProcessPurchaseCommandValidator.cs
│   │   │   ├── ProcessSale/
│   │   │   │   ├── ProcessSaleCommand.cs
│   │   │   │   ├── ProcessSaleCommandHandler.cs
│   │   │   │   └── ProcessSaleCommandValidator.cs
│   │   │   └── TransferStock/
│   │   │       ├── TransferStockCommand.cs
│   │   │       ├── TransferStockCommandHandler.cs
│   │   │       └── TransferStockCommandValidator.cs
│   │   │
│   │   ├── DTOs/
│   │   │   └── StockMovementDto.cs
│   │   │
│   │   └── Queries/
│   │       └── GetMovementHistory/
│   │           ├── GetMovementHistoryQuery.cs
│   │           ├── GetMovementHistoryQueryHandler.cs
│   │           ├── GetStockByWarehouseQuery.cs
│   │           └── GetStockByWarehouseQueryHandler.cs
│   │
│   ├── Suppliers/
│   │   ├── Commands/
│   │   │   ├── CreateSupplier/
│   │   │   │   ├── CreateSupplierCommand.cs
│   │   │   │   ├── CreateSupplierCommandHandler.cs
│   │   │   │   └── CreateSupplierCommandValidator.cs
│   │   │   └── UpdateSupplier/
│   │   │       ├── UpdateSupplierCommand.cs
│   │   │       ├── UpdateSupplierCommandHandler.cs
│   │   │       └── UpdateSupplierCommandValidator.cs
│   │   │
│   │   ├── DTOs/
│   │   │   └── SupplierDto.cs
│   │   │
│   │   └── Queries/
│   │       ├── GetSupplierById/
│   │       │   ├── GetSupplierByIdQuery.cs
│   │       │   └── GetSupplierByIdQueryHandler.cs
│   │       └── GetSuppliers/
│   │           ├── GetSuppliersQuery.cs
│   │           └── GetSuppliersQueryHandler.cs
│   │
│   ├── Warehouses/
│   │   ├── Commands/
│   │   │   ├── CreateWarehouse/
│   │   │   │   ├── CreateWarehouseCommand.cs
│   │   │   │   ├── CreateWarehouseCommandHandler.cs
│   │   │   │   └── CreateWarehouseCommandValidator.cs
│   │   │   └── UpdateWarehouse/
│   │   │       ├── UpdateWarehouseCommand.cs
│   │   │       ├── UpdateWarehouseCommandHandler.cs
│   │   │       └── UpdateWarehouseCommandValidator.cs
│   │   │
│   │   ├── DTOs/
│   │   │   └── WarehouseDto.cs
│   │   │
│   │   └── Queries/
│   │       ├── GetWarehouseById/
│   │       │   ├── GetWarehouseByIdQuery.cs
│   │       │   └── GetWarehouseByIdQueryHandler.cs
│   │       └── GetWarehouses/
│   │           ├── GetWarehousesQuery.cs
│   │           └── GetWarehousesQueryHandler.cs
│   │
│   ├── DependencyInjection.cs
│   └── InventorySystem.Application.csproj
│
├── InventorySystem.Domain/
│   ├── Entities/
│   │   ├── Common/
│   │   │   ├── AuditableEntity.cs
│   │   │   ├── BaseEntity.cs
│   │   │   └── ISoftDeletable.cs
│   │   ├── Category.cs
│   │   ├── Product.cs
│   │   ├── StockMovement.cs
│   │   ├── Supplier.cs
│   │   ├── User.cs
│   │   └── Warehouse.cs
│   │
│   ├── Enums/
│   │   ├── MovementType.cs
│   │   ├── ProductStatus.cs
│   │   └── UserRole.cs
│   │
│   ├── Exceptions/
│   │   ├── DomainException.cs
│   │   ├── InsufficientStockException.cs
│   │   ├── InvalidOperationDomainException.cs
│   │   └── InvalidPriceException.cs
│   │
│   ├── ValueObjects/
│   │   ├── Address.cs
│   │   ├── Money.cs
│   │   └── StockLevel.cs
│   │
│   └── InventorySystem.Domain.csproj
│
├── InventorySystem.Infrastructure/
│   ├── Identity/
│   │   ├── JwtSettings.cs
│   │   ├── PasswordHasher.cs
│   │   └── TokenService.cs
│   │
│   ├── Persistence/
│   │   ├── Configurations/
│   │   │   ├── CategoryConfiguration.cs
│   │   │   ├── ProductConfiguration.cs
│   │   │   ├── StockMovementConfiguration.cs
│   │   │   ├── SupplierConfiguration.cs
│   │   │   ├── UserConfiguration.cs
│   │   │   └── WarehouseConfiguration.cs
│   │   │
│   │   ├── Interceptors/
│   │   │   ├── AuditableEntityInterceptor.cs
│   │   │   └── SoftDeleteInterceptor.cs
│   │   │
│   │   ├── Migrations/
│   │   │
│   │   ├── Repositories/
│   │   │   ├── GenericRepository.cs
│   │   │   ├── ProductRepository.cs
│   │   │   └── StockMovementRepository.cs
│   │   ├── ApplicationDbContext.cs
│   │   ├── ApplicationDbContextSeed.cs
│   │   └── UnitOfWork.cs
│   │
│   ├── Services/
│   │   ├── CurrentUserService.cs
│   │   └── DateTimeService.cs
│   │
│   ├── DependencyInjection.cs
│   └── InventorySystem.Infrastructure.csproj
│
└── InventorySystem.sln 
```

---

## 🚀 **Instalación y Ejecución**

### **Prerrequisitos**
- **.NET 9 SDK** ([Descargar aquí](https://dotnet.microsoft.com/download))
- **PostgreSQL 14+** ([Descargar aquí](https://www.postgresql.org/download/))
- **Visual Studio 2022** / **VS Code** / **Rider**
- **EF Core Tools**: `dotnet tool install --global dotnet-ef`

### **1. Clonar el repositorio**
```bash
git clone https://github.com/bolanosjosue/enterprise-inventory-system.git
cd enterprise-inventory-system/backend
```

### **2. Configurar la base de datos**

Crea la base de datos en PostgreSQL:
```sql
CREATE DATABASE InventorySystemDb;
```

### **3. Configurar variables de entorno**

Edita `InventorySystem.API/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=InventorySystemDb;Username=postgres;Password=TU_PASSWORD"
  },
  "JwtSettings": {
    "Secret": "tu-secret-key-de-al-menos-32-caracteres-aqui",
    "ExpirationInHours": 8,
    "Issuer": "InventorySystemAPI",
    "Audience": "InventorySystemClient"
  }
}
```

⚠️ **Nota:** `appsettings.Development.json` está en `.gitignore` para no exponer secretos.

### **4. Aplicar migraciones**
```bash
dotnet ef database update --project InventorySystem.Infrastructure --startup-project InventorySystem.API
```

Esto creará:
- ✅ Todas las tablas
- ✅ Relaciones e índices
- ✅ Datos de prueba (seed):
  - 1 Admin: `admin@inventory.com` / `Admin123!`
  - 3 Categorías
  - 3 Proveedores
  - 2 Bodegas
  - 10 Productos
  - Movimientos de ejemplo

### **5. Ejecutar la aplicación**
```bash
dotnet run --project InventorySystem.API
```

O desde Visual Studio: `F5`

La API estará disponible en:
- **HTTPS**: `https://localhost:7081`
- **HTTP**: `http://localhost:5094`
- **Swagger**: `http://localhost:5094` (redirige automáticamente)

---

## 📚 **Documentación de API (Swagger)**

Una vez ejecutada la aplicación, accede a:
```
http://localhost:5094
```

### **Autenticación en Swagger**

1. Ir al endpoint `POST /api/auth/login`
2. Usar credenciales:
```json
   {
     "email": "admin@inventory.com",
     "password": "Admin123!"
   }
```
3. Copiar el `token` de la respuesta
4. Hacer clic en el botón **"Authorize"** (🔒 arriba a la derecha)
5. Ingresar: `Bearer {tu-token-aqui}`
6. Ahora puedes probar todos los endpoints protegidos

---

## 🔐 **Endpoints Principales**

### **Authentication**
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registrar nuevo usuario

### **Products**
- `GET /api/products` - Listar productos (con paginación, búsqueda y filtros)
- `GET /api/products/{id}` - Obtener producto por ID
- `GET /api/products/low-stock` - Productos con stock bajo
- `POST /api/products` - Crear producto (Admin, Supervisor)
- `PUT /api/products/{id}` - Actualizar producto (Admin, Supervisor)
- `DELETE /api/products/{id}` - Eliminar producto (Admin)

### **Stock Movements**
- `GET /api/stockmovements` - Historial de movimientos
- `GET /api/stockmovements/warehouse/{id}/stock` - Stock actual por bodega
- `POST /api/stockmovements/purchase` - Registrar compra (Admin, Supervisor)
- `POST /api/stockmovements/sale` - Registrar venta (Admin, Supervisor, Operator)
- `POST /api/stockmovements/transfer` - Transferir entre bodegas (Admin, Supervisor)

### **Categories, Suppliers, Warehouses**
- CRUD completo para cada entidad con permisos por rol

### **Users** (Solo Admin)
- `GET /api/users` - Listar usuarios
- `PUT /api/users/{id}/role` - Cambiar rol de usuario
- `PUT /api/users/{id}/toggle-status` - Activar/Desactivar usuario

---

## 🎯 **Roles y Permisos**

| Acción | Admin | Supervisor | Operator | Viewer |
|--------|-------|------------|----------|--------|
| Ver Dashboard | ✅ | ✅ | ✅ | ✅ |
| Ver Productos | ✅ | ✅ | ✅ | ✅ |
| Crear/Editar Productos | ✅ | ✅ | ❌ | ❌ |
| Eliminar Productos | ✅ | ❌ | ❌ | ❌ |
| Gestionar Categorías | ✅ | ✅ | ❌ | ❌ |
| Gestionar Proveedores | ✅ | ✅ | ❌ | ❌ |
| Gestionar Bodegas | ✅ | ❌ | ❌ | ❌ |
| Registrar Compra | ✅ | ✅ | ❌ | ❌ |
| Registrar Venta | ✅ | ✅ | ✅ | ❌ |
| Transferir Stock | ✅ | ✅ | ❌ | ❌ |
| Gestionar Usuarios | ✅ | ❌ | ❌ | ❌ |

---

## 🔒 **Seguridad**

### **Buenas Prácticas Implementadas**

✅ Passwords nunca en texto plano (BCrypt con salt)  
✅ JWT con expiración configurable  
✅ Secrets en archivos gitignored  
✅ Validación de input en todos los endpoints  
✅ Rate limiting para prevenir ataques  
✅ CORS configurado correctamente  
✅ HTTPS en producción  
✅ Soft delete para mantener auditoría  
✅ Optimistic concurrency para evitar race conditions  

---

### **Recomendaciones para Producción**

- Usar certificados SSL válidos
- Configurar rate limiting más estricto
- Implementar refresh tokens
- Logs estructurados (Serilog)
- Monitoreo con Application Insights
- Backups automáticos de base de datos
- Variables de entorno en servidor (no appsettings)

---

## 📄 **Licencia**

Este proyecto es de código abierto bajo la licencia MIT.

---

## 👨‍💻 **Autor**

**Josué Bolaños Urbina**  
Software Engineer

- GitHub: https://github.com/bolanosjosue  
- Portafolio: https://josuebolanos.netlify.app

⭐ **Si este proyecto te fue útil, considera darle una estrella en GitHub**
