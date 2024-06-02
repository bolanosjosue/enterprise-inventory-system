using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Authentication.Services;
using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;


namespace InventorySystem.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<LoginDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<User> _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IRepository<User> userRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

        if (emailExists)
            return Result.Failure<LoginDto>("Email already exists");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = User.Create(
            request.Email,
            hashedPassword,
            request.FullName,
            UserRole.Operator
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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