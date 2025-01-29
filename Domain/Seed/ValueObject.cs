namespace Domain.Seed;

public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(ValueObject<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetType() == other.GetType() && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ValueObject<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
    }

    public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
    {
        return !Equals(left, right);
    }
}