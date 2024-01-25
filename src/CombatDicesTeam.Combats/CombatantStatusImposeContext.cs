using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Base implementation of the context.
/// </summary>
public sealed class CombatantStatusImposeContext : ICombatantStatusImposeContext
{
    public CombatantStatusImposeContext(CombatEngineBase combat)
    {
        Combat = combat;
    }

    /// <inheritdoc />
    public CombatEngineBase Combat { get; }

    /// <inheritdoc />
    public void ImposeCombatantStatus(ICombatant target, ICombatantStatusSource source,
        ICombatantStatusFactory combatantStatusFactory)
    {
        Combat.ImposeCombatantStatus(target, combatantStatusFactory.Create(source));
    }
}