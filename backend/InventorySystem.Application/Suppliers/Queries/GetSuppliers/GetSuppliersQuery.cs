using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Suppliers.DTOs;
using MediatR;

namespace InventorySystem.Application.Suppliers.Queries.GetSuppliers;

public record GetSuppliersQuery : IRequest<Result<List<SupplierDto>>>;