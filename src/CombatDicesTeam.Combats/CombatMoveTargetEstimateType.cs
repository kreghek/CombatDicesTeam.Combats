namespace CombatDicesTeam.Combats;

/// <summary>
/// Type of combat target estimation. 
/// </summary>
public enum CombatMoveTargetEstimateType
{
    /// <summary>
    /// Target fully defined.
    /// </summary>
    Exactly,
    
    /// <summary>
    /// Used for random. This combatant can be target.
    /// </summary>
    Approximately
}