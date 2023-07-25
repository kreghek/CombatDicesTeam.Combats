using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class DamageEffect : IEffect
{
    public DamageEffect(ITargetSelector selector, DamageType damageType, GenericRange<int> damage)
    {
        Selector = selector;
        DamageType = damageType;
        Damage = damage;
        Modifiers = ArraySegment<IDamageEffectModifier>.Empty;
    }

    public DamageEffect(ITargetSelector selector, DamageType damageType, GenericRange<int> damage,
        IReadOnlyList<IDamageEffectModifier> modifiers)
    {
        Selector = selector;
        DamageType = damageType;
        Damage = damage;
        Modifiers = modifiers;
    }

    public GenericRange<int> Damage { get; }
    public DamageType DamageType { get; }
    public IReadOnlyList<IDamageEffectModifier> Modifiers { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new DamageEffectInstance(this);
    }
}