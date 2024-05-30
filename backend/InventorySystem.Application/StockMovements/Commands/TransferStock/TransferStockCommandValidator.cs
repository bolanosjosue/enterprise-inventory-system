using FluentValidation;

namespace InventorySystem.Application.StockMovements.Commands.TransferStock;

public class TransferStockCommandValidator : AbstractValidator<TransferStockCommand>
{
    public TransferStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required");

        RuleFor(x => x.SourceWarehouseId)
            .NotEmpty().WithMessage("Source warehouse is required");

        RuleFor(x => x.DestinationWarehouseId)
            .NotEmpty().WithMessage("Destination warehouse is required");

        RuleFor(x => x)
            .Must(x => x.SourceWarehouseId != x.DestinationWarehouseId)
            .WithMessage("Source and destination warehouses must be different");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative");

        RuleFor(x => x.Reference)
            .NotEmpty().WithMessage("Reference is required")
            .MaximumLength(100).WithMessage("Reference must not exceed 100 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}