namespace CombatDicesTeam.Combats.Effects;

public sealed record CounterCombatantStatusSource(ICombatantStatus Status) : ICombatantStatusSource;