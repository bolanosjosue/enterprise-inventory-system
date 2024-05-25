using InventorySystem.Application.Common.Models;
using InventorySystem.Application.Warehouses.DTOs;
using MediatR;

namespace InventorySystem.Application.Warehouses.Queries.GetWarehouses;

public record GetWarehousesQuery : IRequest<Result<List<WarehouseDto>>>;