using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats;

public interface ICombatantStatusImposeContext
{
    CombatEngineBase Combat { get; }
    void ImposeCombatantStatus(ICombatant target, ICombatantStatusSource source, ICombatantStatusFactory combatantStatusFactory);
}