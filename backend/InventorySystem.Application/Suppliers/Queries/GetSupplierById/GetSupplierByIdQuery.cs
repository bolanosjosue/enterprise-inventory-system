using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Suppliers.DTOs;
using MediatR;

namespace InventorySystem.Application.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdQuery(Guid Id) : IRequest<Result<SupplierDto>>;