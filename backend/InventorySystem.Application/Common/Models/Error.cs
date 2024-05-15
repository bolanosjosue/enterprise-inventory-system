namespace InventorySystem.Application.Common.Models;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public static Error NotFound(string entityName, object key)
        => new("Error.NotFound", $"{entityName} with id '{key}' was not found");

    public static Error Validation(string message)
        => new("Error.Validation", message);

    public static Error Conflict(string message)
        => new("Error.Conflict", message);

    public static Error Failure(string message)
        => new("Error.Failure", message);
}