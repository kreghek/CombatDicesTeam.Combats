namespace CombatDicesTeam.Combats.Effects;

public sealed record CombatMovementCombatantStatusSource(ICombatant Actor) : ICombatantStatusSource;