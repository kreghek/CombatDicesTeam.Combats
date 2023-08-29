namespace CombatDicesTeam.Combats;

public interface IStatModifier
{
    int Value { get; }

    IStatModifierSource Source { get; }
}