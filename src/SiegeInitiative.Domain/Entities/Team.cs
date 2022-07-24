using SiegeInitiative.Domain.Entities.Base;
using SiegeInitiative.Domain.Entities.Enums;

namespace SiegeInitiative.Domain.Entities;

public sealed class Team : Entity<int>
{
    public string? Name { get; set; }
    public string? Tag { get; set; }
    public Tier Tier { get; set; }
    public Region Region { get; set; }
    public string? Nationality { get; set; }
    public IEnumerable<Player> Players { get; set; }
}