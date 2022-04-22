namespace SiegeInitiative.Domain.Entities.Base;

public abstract class Entity<TKey>
{
    public TKey? Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset DeletedAt { get; set; }
}