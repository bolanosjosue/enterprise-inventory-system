using InventorySystem.Domain.Entities.Common;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities;

public class User : AuditableEntity, ISoftDeletable
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash, string fullName, UserRole role = UserRole.Operator)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required", nameof(fullName));

        return new User
        {
            Email = email.ToLowerInvariant().Trim(),
            PasswordHash = passwordHash,
            FullName = fullName.Trim(),
            Role = role,
            IsActive = true
        };
    }

    public void UpdateRole(UserRole newRole)
    {
        Role = newRole;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void SoftDelete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}