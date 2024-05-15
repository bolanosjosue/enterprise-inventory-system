namespace InventorySystem.Domain.ValueObjects;

public sealed class StockLevel : IEquatable<StockLevel>
{
    public int Quantity { get; }
    public int MinimumLevel { get; }
    public int MaximumLevel { get; }

    private StockLevel(int quantity, int minimumLevel, int maximumLevel)
    {
        Quantity = quantity;
        MinimumLevel = minimumLevel;
        MaximumLevel = maximumLevel;
    }

    public static StockLevel Create(int quantity, int minimumLevel, int maximumLevel)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative");

        if (minimumLevel < 0)
            throw new ArgumentException("Minimum level cannot be negative");

        if (maximumLevel < minimumLevel)
            throw new ArgumentException("Maximum must be greater than minimum");

        return new StockLevel(quantity, minimumLevel, maximumLevel);
    }

    public bool IsLow() => Quantity <= MinimumLevel;
    public bool IsOverstocked() => Quantity >= MaximumLevel;
    public bool IsOptimal() => Quantity > MinimumLevel && Quantity < MaximumLevel;
    public bool CanDecrease(int amount) => Quantity >= amount;

    public StockLevel Increase(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");

        return new StockLevel(Quantity + amount, MinimumLevel, MaximumLevel);
    }

    public StockLevel Decrease(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");

        if (!CanDecrease(amount))
            throw new InvalidOperationException($"Insufficient stock. Available: {Quantity}, Requested: {amount}");

        return new StockLevel(Quantity - amount, MinimumLevel, MaximumLevel);
    }

    public bool Equals(StockLevel? other)
    {
        if (other is null) return false;
        return Quantity == other.Quantity
            && MinimumLevel == other.MinimumLevel
            && MaximumLevel == other.MaximumLevel;
    }

    public override bool Equals(object? obj) => obj is StockLevel level && Equals(level);
    public override int GetHashCode() => HashCode.Combine(Quantity, MinimumLevel, MaximumLevel);
    public static bool operator ==(StockLevel? left, StockLevel? right) => Equals(left, right);
    public static bool operator !=(StockLevel? left, StockLevel? right) => !Equals(left, right);
}