namespace CombatDicesTeam.Combats;

public sealed record StatModifier(int Value, IStatModifierSource Source) : IStatModifier;