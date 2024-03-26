using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantStatuses;

[PublicAPI]
public sealed class AlliesAuraTargetSelector : IAuraTargetSelector
{
    public bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant, IAuraTargetSelectorContext context)
    {
        return auraOwner.IsPlayerControlled != testCombatant.IsPlayerControlled;
    }
}