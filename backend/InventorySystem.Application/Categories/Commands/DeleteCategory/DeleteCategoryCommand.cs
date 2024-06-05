using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result>;