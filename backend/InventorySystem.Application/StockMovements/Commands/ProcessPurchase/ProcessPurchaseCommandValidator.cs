using FluentValidation;

namespace InventorySystem.Application.StockMovements.Commands.ProcessPurchase;

public class ProcessPurchaseCommandValidator : AbstractValidator<ProcessPurchaseCommand>
{
    public ProcessPurchaseCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse is required");

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