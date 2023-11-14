namespace CombatDicesTeam.Combats;

/// <summary>
/// Context to calculate combat state.
/// </summary>
public interface ICombatStateStrategyContext
{
    /// <summary>
    /// Combat units.
    /// </summary>
    IReadOnlyCollection<ICombatant> Combatants { get; }
}