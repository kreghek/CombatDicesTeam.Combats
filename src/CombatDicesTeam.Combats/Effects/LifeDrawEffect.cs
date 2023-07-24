using CombatDicesTeam.Utils;

namespace Core.Combats.Effects;

public sealed class LifeDrawEffect : IEffect
{
    public LifeDrawEffect(ITargetSelector selector, Range<int> damage)
    {
        Selector = selector;
        Damage = damage;
    }

    public Range<int> Damage { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new LifeDrawEffectInstance(this);
    }
}