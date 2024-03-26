namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// Base implementation of aura selection context. 
/// </summary>
public sealed record AuraTargetSelectorContext(CombatEngineBase Combat):IAuraTargetSelectorContext;