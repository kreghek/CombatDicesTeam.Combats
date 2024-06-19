namespace CombatDicesTeam.Combats;

public sealed record CombatantMovementStatChangingSource(ICombatant Damager) : IStatChangingSource;