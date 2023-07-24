namespace Core.Combats;

public sealed class CombatantStat : IUnitStat
{
    public CombatantStat(ICombatantStatType type, IStatValue value)
    {
        Type = type;
        Value = value;
    }

    public ICombatantStatType Type { get; }
    public IStatValue Value { get; }
}