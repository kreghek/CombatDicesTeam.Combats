namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// The context to select aura target based on combat elements.
/// For example, use a combatant's position on the field.
/// </summary>
public interface IAuraTargetSelectorContext
{
    /// <summary>
    /// Combat.
    /// </summary>
    CombatEngineBase Combat { get; }
}