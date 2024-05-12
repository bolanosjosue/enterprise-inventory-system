# InventoryPro – Enterprise Inventory System (Backend)

Sistema de gestión de inventario empresarial construido con **Clean Architecture** y **CQRS**, enfocado en buenas prácticas y diseño de dominio.

## Stack Tecnológico
- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- MediatR (CQRS)
- FluentValidation

## Arquitectura
Clean Architecture en 4 capas:
- **Domain:** Entidades y reglas de negocio
- **Application:** Casos de uso, CQRS y DTOs
- **Infrastructure:** Persistencia y servicios externos
- **API:** Controllers y endpoints REST

## Instalación
```bash
dotnet restore
dotnet build
