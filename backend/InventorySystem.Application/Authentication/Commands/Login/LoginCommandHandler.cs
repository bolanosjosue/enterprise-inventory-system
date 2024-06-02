using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Authentication.Services;
using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IApplicationDbContext context,
        ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

        if (user == null)
            return Result.Failure<LoginDto>("Invalid email or password");

        if (!user.IsActive)
            return Result.Failure<LoginDto>("User account is inactive");

        var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isValidPassword)
            return Result.Failure<LoginDto>("Invalid email or password");

        user.RecordLogin();
        await _context.SaveChangesAsync(cancellationToken);

        var token = _tokenService.GenerateToken(user);

        var loginDto = new LoginDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        };

        return Result.Success(loginDto);
    }
}