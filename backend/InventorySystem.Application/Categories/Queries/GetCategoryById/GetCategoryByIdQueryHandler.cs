using InventorySystem.Application.Categories.DTOs;
using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category == null)
            return Result.Failure<CategoryDto>("Category not found");

        var dto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductCount = category.Products.Count,
            CreatedAt = category.CreatedAt
        };

        return Result.Success(dto);
    }
}