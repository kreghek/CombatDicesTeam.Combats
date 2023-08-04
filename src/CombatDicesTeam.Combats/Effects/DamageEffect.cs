using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class DamageEffect : IEffect
{
    private readonly DamageEffectConfig _statConfig;

    public DamageEffect(ITargetSelector selector, DamageType damageType, GenericRange<int> damage,
        DamageEffectConfig statConfig)
    {
        _statConfig = statConfig;

        Selector = selector;
        DamageType = damageType;
        Damage = damage;
        Modifiers = ArraySegment<IDamageEffectModifier>.Empty;
    }

    public DamageEffect(ITargetSelector selector, DamageType damageType, GenericRange<int> damage,
        DamageEffectConfig statConfig,
        IReadOnlyList<IDamageEffectModifier> modifiers)
    {
        _statConfig = statConfig;

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
        return new DamageEffectInstance(this, _statConfig);
    }
}