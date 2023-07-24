namespace Core.Combats;

public sealed class CombatantStatsConfig
{
    private readonly IDictionary<ICombatantStatType, IStatValue> _stats;

    public CombatantStatsConfig()
    {
        _stats = new Dictionary<ICombatantStatType, IStatValue>
        {
            {
                CombatantStatTypes.ShieldPoints, new CombatantStatValue(new StatValue(1))
            },
            {
                CombatantStatTypes.HitPoints, new CombatantStatValue(new StatValue(3))
            },
            {
                CombatantStatTypes.Resolve, new CombatantStatValue(new StatValue(5))
            },
            {
                CombatantStatTypes.Maneuver, new CombatantStatValue(new StatValue(1))
            },
            {
                CombatantStatTypes.Defense, new CombatantStatValue(new StatValue(0))
            }
        };
    }

    public IReadOnlyCollection<IUnitStat> GetStats()
    {
        return _stats.Select(x => new CombatantStat(x.Key, x.Value)).ToArray();
    }

    public void SetValue(ICombatantStatType statType, int value)
    {
        _stats[statType].ChangeBase(value);
        _stats[statType].CurrentChange(value);
    }

    public void SetValue(ICombatantStatType statType, IStatValue value)
    {
        _stats[statType] = value;
    }
}