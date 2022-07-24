namespace SiegeInitiative.Domain.Entities.Base;

public abstract class Entity<TKey>
{
    public TKey? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}