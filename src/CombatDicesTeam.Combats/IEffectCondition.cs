namespace CombatDicesTeam.Combats;

/// <summary>
/// Condition of combat movement effect.
/// </summary>
public interface IEffectCondition
{
    /// <summary>
    /// Check must effect be applied.
    /// </summary>
    bool Check();
}