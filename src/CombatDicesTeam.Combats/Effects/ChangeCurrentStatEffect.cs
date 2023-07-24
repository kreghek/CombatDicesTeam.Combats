using CombatDicesTeam.Utils;

namespace CombatDicesTeam.Combats.Effects;

public sealed class ChangeCurrentStatEffect : IEffect
{
    public ChangeCurrentStatEffect(ITargetSelector selector, ICombatantStatType statType,
        Range<int> statValue)
    {
        TargetStatType = statType;
        Selector = selector;
        StatValue = statValue;
    }

    public Range<int> StatValue { get; }
    public ICombatantStatType TargetStatType { get; }
    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new ChangeCurrentStatEffectInstance(this);
    }
}