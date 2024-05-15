namespace InventorySystem.Domain.Exceptions;

public class InvalidPriceException : DomainException
{
    public decimal Price { get; }

    public InvalidPriceException(decimal price)
        : base($"Invalid price: {price}. Price must be greater than zero.")
    {
        Price = price;
    }
}