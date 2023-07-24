namespace CombatDicesTeam.Combats.Effects;

public sealed class PeriodicEffect : IEffect
{
    public PeriodicEffect(ITargetSelector selector, IEffect baseEffect, ICombatantStatusLifetime lifetime)
    {
        Selector = selector;
        BaseEffect = baseEffect;
        Lifetime = lifetime;
    }

    public IEffect BaseEffect { get; }
    public ICombatantStatusLifetime Lifetime { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new PeriodicEffectInstance(this);
    }
}