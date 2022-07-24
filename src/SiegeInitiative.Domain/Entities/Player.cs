using SiegeInitiative.Domain.Entities.Base;
using SiegeInitiative.Domain.Entities.Enums;

namespace SiegeInitiative.Domain.Entities;

public sealed class Player : Entity<int>
{
    public string Name { get; set; }
    public string Nickname { get; set; }
    public string Nationality { get; set; }
    public Function Function { get; set; }
    public bool FreeAgent { get; set; }
    public Team Team { get; set; }
    public DateTime Birth { get; set; }
}