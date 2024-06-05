using InventorySystem.Application.Categories.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest<Result<CategoryDto>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}