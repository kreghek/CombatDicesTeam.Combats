namespace CombatDicesTeam.Combats;

/// <summary>
/// Stat modifier.
/// </summary>
public interface IStatModifier
{
    /// <summary>
    /// Value to modify.
    /// </summary>
    int Value { get; }

    /// <summary>
    /// Modification source to display stat modifications on a clients.
    /// </summary>
    IStatModifierSource Source { get; }
}