using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats;

// ReSharper disable once InconsistentNaming
public sealed class CombatantStatusImposeContext : ICombatantStatusImposeContext
{
    public CombatantStatusImposeContext(CombatEngineBase combat)
    {
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }

    public void ImposeCombatantStatus(ICombatant target, ICombatantStatusSource source, ICombatantStatusFactory combatantStatusFactory)
    {
        Combat.ImposeCombatantStatus(target, combatantStatusFactory.Create(source));
    }
}