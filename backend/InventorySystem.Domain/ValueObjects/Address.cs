namespace InventorySystem.Domain.ValueObjects;

public sealed class Address : IEquatable<Address>
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string? PostalCode { get; }

    private Address(string street, string city, string state, string country, string? postalCode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }

    public static Address Create(string street, string city, string state, string country, string? postalCode = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required");

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required");

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required");

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required");

        return new Address(
            street.Trim(),
            city.Trim(),
            state.Trim(),
            country.Trim(),
            postalCode?.Trim()
        );
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Street == other.Street
            && City == other.City
            && State == other.State
            && Country == other.Country
            && PostalCode == other.PostalCode;
    }

    public override bool Equals(object? obj) => obj is Address address && Equals(address);
    public override int GetHashCode() => HashCode.Combine(Street, City, State, Country, PostalCode);
    public static bool operator ==(Address? left, Address? right) => Equals(left, right);
    public static bool operator !=(Address? left, Address? right) => !Equals(left, right);
    public override string ToString() => $"{Street}, {City}, {State}, {Country}";
}