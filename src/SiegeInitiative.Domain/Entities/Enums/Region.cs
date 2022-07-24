using System.ComponentModel;

namespace SiegeInitiative.Domain.Entities.Enums;

public enum Region
{
    /// <summary>
    /// Latin america region
    /// </summary>
    [Description("LATAM - Latin America")]
    LATAM = 1,

    /// <summary>
    /// Asia-Pacific region
    /// </summary>
    [Description("APAC - Asia-Pacific")]
    APAC = 2,

    /// <summary>
    /// Europe region
    /// </summary>
    [Description("EU - Europe")]
    EU = 3,

    /// <summary>
    /// North america region
    /// </summary>
    [Description("NA - North America")]
    NA = 4
}