using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats;

public interface ICombatantStatusImposeContext
{
    CombatEngineBase Combat { get; }

    /// <summary>
    /// Impose the status to target combatant.
    /// </summary>
    /// <param name="target">Target combatant.</param>
    /// <param name="source">Source os the status.</param>
    /// <param name="combatantStatusFactory">Status factory for creating a status in the moment of imposing.</param>
    void ImposeCombatantStatus(ICombatant target, ICombatantStatusSource source,
        ICombatantStatusFactory combatantStatusFactory);
}