namespace Domain.Seed;

public class Entity<T>(T id)
{
    public T Id { get; init; } = id;
}