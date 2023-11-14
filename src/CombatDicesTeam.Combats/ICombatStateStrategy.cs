namespace CombatDicesTeam.Combats;

/// <summary>
/// Calculates the combat's state in different ways.
/// </summary>
public interface ICombatStateStrategy
{
    /// <summary>
    /// Calculate current combat state.
    /// </summary>
    /// <param name="context"> Combat context to calculate state, </param>
    /// <returns>Combat state using current context.</returns>
    ICombatState CalculateCurrentState(ICombatStateStrategyContext context);
}