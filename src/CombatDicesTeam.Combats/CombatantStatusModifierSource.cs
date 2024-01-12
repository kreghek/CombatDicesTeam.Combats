namespace CombatDicesTeam.Combats;

public sealed record CombatantStatusModifierSource(ICombatantStatus Status) : IStatModifierSource;