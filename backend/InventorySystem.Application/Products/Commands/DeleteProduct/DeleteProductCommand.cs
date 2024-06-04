using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Result>;