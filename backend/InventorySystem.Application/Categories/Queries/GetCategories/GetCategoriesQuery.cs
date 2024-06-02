using InventorySystem.Application.Categories.DTOs;
using InventorySystem.Application.Common.Models;
using MediatR;

namespace InventorySystem.Application.Categories.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<Result<List<CategoryDo>>>;