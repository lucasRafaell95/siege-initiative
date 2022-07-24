namespace SiegeInitiative.Domain.Entities.Enums;

public enum Function
{
    /// <summary>
    /// Player who is generally droned into the building 
    /// first, looking to get early kills.
    /// </summary>
    EntryFragger = 1,

    /// <summary>
    /// Player who can play either role depending on 
    /// situation and fill for the team.
    /// </summary>
    Flex = 2,

    /// <summary>
    /// Player who plays outside of the objective room, 
    /// trying to slow the attackers down, trying to flank them.
    /// </summary>
    Roamer = 3,

    /// <summary>
    /// Player who plays more defensively, drones for 
    /// others, plants the bomb, usually a hard breacher.
    /// </summary>
    Support = 4,

    /// <summary>
    /// Member of the team responsible for developing tactical 
    /// techniques and counters
    /// </summary>
    Coach = 5
}