using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Authentication.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Where(u => !u.IsDeleted)
            .OrderBy(u => u.FullName)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                LastLoginAt = u.LastLoginAt,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(users);
    }
}