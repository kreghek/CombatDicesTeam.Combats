using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats;

public interface ICombatantStatusImposeContext
{
    void ImposeCombatantStatus(ICombatant target, ICombatantStatusFactory combatantStatusFactory);

    CombatEngineBase Combat { get; }
}