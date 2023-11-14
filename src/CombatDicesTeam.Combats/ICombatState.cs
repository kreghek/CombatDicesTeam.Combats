namespace CombatDicesTeam.Combats;

/// <summary>
/// Combat state.
/// </summary>
public interface ICombatState
{
    /// <summary>
    /// Is combat finished in this state.
    /// </summary>
    bool IsFinalState { get; }
}