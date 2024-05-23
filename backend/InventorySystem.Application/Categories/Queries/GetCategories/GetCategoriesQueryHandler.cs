using InventorySystem.Application.Categories.DTOs;
using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .Include(c => c.Products)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products.Count,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(categories);
    }
}