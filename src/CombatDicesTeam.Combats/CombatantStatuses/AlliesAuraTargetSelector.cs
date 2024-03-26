using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// Aura imposes to all allies.
/// </summary>
[PublicAPI]
public sealed class AlliesAuraTargetSelector : IAuraTargetSelector
{
    public bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant, IAuraTargetSelectorContext context)
    {
        return auraOwner.IsPlayerControlled != testCombatant.IsPlayerControlled;
    }
}