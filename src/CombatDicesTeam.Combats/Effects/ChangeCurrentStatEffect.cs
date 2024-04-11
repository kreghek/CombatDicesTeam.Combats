using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class ChangeCurrentStatEffect : IEffect
{
    public ChangeCurrentStatEffect(ITargetSelector selector, ICombatantStatType statType,
        GenericRange<int> statValue)
    {
        TargetStatType = statType;
        Selector = selector;
        StatValue = statValue;
    }

    public GenericRange<int> StatValue { get; }
    public ICombatantStatType TargetStatType { get; }
    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions { get; init; } = Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new ChangeCurrentStatEffectInstance(this);
    }
}