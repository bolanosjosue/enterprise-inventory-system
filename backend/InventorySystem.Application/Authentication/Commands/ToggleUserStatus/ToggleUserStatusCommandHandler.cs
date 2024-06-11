using InventorySystem.Application.Authentication.DTOs;
using InventorySystem.Application.Common.Interfaces;
using InventorySystem.Application.Common.Models;
using InventorySystem.Domain.Entities;
using MediatR;

namespace InventorySystem.Application.Authentication.Commands.ToggleUserStatus;

public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, Result<UserDto>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleUserStatusCommandHandler(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure<UserDto>("User not found");

        if (user.IsActive)
            user.Deactivate();
        else
            user.Activate();

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };

        return Result.Success(userDto);
    }
}