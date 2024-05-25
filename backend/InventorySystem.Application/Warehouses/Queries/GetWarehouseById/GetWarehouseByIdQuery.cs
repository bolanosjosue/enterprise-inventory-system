using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using MediatR;

namespace InventorySystem.Application.Warehouses.Queries.GetWarehouseById;

public record GetWarehouseByIdQuery(Guid Id) : IRequest<Result<WarehouseDto>>;