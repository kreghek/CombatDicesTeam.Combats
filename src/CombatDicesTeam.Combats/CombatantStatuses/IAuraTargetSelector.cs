namespace CombatDicesTeam.Combats.CombatantStatuses;

public interface IAuraTargetSelector
{
    bool IsCombatantUnderAura(ICombatant auraOwner, ICombatant testCombatant, IAuraTargetSelectorContext context);
}

public interface IAuraTargetSelectorContext
{
    CombatEngineBase Combat { get; }
}

public sealed record AuraTargetSelectorContext(CombatEngineBase Combat):IAuraTargetSelectorContext;