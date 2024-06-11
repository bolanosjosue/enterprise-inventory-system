using FluentValidation;

namespace InventorySystem.Application.Authentication.Commands.UpdateUserRole;

public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(role => new[] { "Admin", "Supervisor", "Operator", "Viewer" }.Contains(role))
            .WithMessage("Invalid role. Must be Admin, Supervisor, Operator, or Viewer");
    }
}