namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class EnemiesAuraTargetSelector: IAuraTargetSelector
{
    public bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant)
    {
        return auraOwner.IsPlayerControlled != testCombatant.IsPlayerControlled;
    }
}