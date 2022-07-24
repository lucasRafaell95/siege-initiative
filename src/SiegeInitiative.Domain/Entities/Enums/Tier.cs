using System.ComponentModel;

namespace SiegeInitiative.Domain.Entities.Enums;

public enum Tier
{
    [Description("S-Tier")]
    S = 1,

    [Description("A-Tier")]
    A = 2,

    [Description("B-Tier")]
    B = 3,

    [Description("C-Tier")]
    C = 4,

    [Description("D-Tier")]
    D = 5
}