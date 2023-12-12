namespace CombatDicesTeam.Combats;

public sealed class CombatantStat : ICombatantStat
{
    public CombatantStat(ICombatantStatType type, IStatValue value)
    {
        Type = type;
        Value = value;
    }

    public ICombatantStatType Type { get; }
    public IStatValue Value { get; }
}