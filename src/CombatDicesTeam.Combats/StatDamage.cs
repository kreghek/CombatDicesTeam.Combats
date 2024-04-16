namespace CombatDicesTeam.Combats;

public sealed record StatDamage(int Amount, int SourceAmount)
{
    public static implicit operator StatDamage(int monoValue) { return new StatDamage(monoValue, monoValue); }
}