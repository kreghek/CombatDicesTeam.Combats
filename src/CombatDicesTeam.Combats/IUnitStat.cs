namespace Core.Combats;

public interface IUnitStat
{
    ICombatantStatType Type { get; }
    IStatValue Value { get; }
}