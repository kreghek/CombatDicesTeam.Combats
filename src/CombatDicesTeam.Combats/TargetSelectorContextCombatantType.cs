namespace CombatDicesTeam.Combats;

/// <summary>
/// Base default implementation of the <see cref="ITargetSelectorContextCombatantType"/>
/// </summary>
/// <param name="DebugName">Name to hep in the debug.</param>
public sealed record TargetSelectorContextCombatantType(string? DebugName = null): ITargetSelectorContextCombatantType;
