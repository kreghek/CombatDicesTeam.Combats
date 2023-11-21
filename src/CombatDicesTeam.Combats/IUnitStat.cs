namespace CombatDicesTeam.Combats;

public interface ICombatantStat
{
    ICombatantStatType Type { get; }
    IStatValue Value { get; }
}