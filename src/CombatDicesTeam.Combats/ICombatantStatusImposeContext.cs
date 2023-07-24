using Core.Combats.CombatantStatuses;

namespace Core.Combats;

public interface ICombatantStatusImposeContext
{
    void ImposeCombatantStatus(ICombatant target, ICombatantStatusFactory combatantStatusFactory);

    CombatEngineBase Combat { get; }
}