namespace CombatDicesTeam.Combats;

public sealed record CombatantDamageSource(ICombatant Damager) : IDamageSource;