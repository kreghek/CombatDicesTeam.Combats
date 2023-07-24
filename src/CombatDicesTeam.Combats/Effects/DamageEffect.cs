using CombatDicesTeam.Utils;

namespace CombatDicesTeam.Combats.Effects;

public sealed class DamageEffect : IEffect
{
    public DamageEffect(ITargetSelector selector, DamageType damageType, Range<int> damage)
    {
        Selector = selector;
        DamageType = damageType;
        Damage = damage;
        Modifiers = ArraySegment<IDamageEffectModifier>.Empty;
    }

    public DamageEffect(ITargetSelector selector, DamageType damageType, Range<int> damage,
        IReadOnlyList<IDamageEffectModifier> modifiers)
    {
        Selector = selector;
        DamageType = damageType;
        Damage = damage;
        Modifiers = modifiers;
    }

    public Range<int> Damage { get; }
    public DamageType DamageType { get; }
    public IReadOnlyList<IDamageEffectModifier> Modifiers { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new DamageEffectInstance(this);
    }
}