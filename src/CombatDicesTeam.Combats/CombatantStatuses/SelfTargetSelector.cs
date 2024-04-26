using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantStatuses;

[PublicAPI]
public sealed class SelfTargetSelector : IAuraTargetSelector
{
    public bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant, IAuraTargetSelectorContext context)
    {
        return auraOwner == testCombatant;
    }
}