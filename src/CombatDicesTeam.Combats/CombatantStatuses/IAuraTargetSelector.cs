namespace CombatDicesTeam.Combats.CombatantStatuses;

public interface IAuraTargetSelector
{
    bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant);
}