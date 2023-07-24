namespace CombatDicesTeam.Combats;

public interface IUnitStat
{
    ICombatantStatType Type { get; }
    IStatValue Value { get; }
}