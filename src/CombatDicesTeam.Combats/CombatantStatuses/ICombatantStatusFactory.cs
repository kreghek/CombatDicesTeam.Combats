namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// Factory to create combatant status
/// </summary>
/// <remarks>
/// Factory is used to create a factory when effects are defined. And create the status itself at the moment
/// the effect is applied.
/// </remarks>
public interface ICombatantStatusFactory
{
    /// <summary>
    /// Creates a status.
    /// </summary>
    /// <param name="source">The source of status. It can be some combatant action of environment.</param>
    /// <returns>Instance of a combatant status.</returns>
    public ICombatantStatus Create(ICombatantStatusSource source);
}