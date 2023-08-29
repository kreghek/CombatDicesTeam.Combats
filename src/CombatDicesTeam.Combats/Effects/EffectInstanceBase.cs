namespace CombatDicesTeam.Combats.Effects;

public abstract class EffectInstanceBase<TEffect> : IEffectInstance where TEffect : IEffect
{
    protected EffectInstanceBase(TEffect baseEffect)
    {
        BaseEffect = baseEffect;
    }

    public TEffect BaseEffect { get; }
    public ITargetSelector Selector => BaseEffect.Selector;

    public virtual void AddModifier(IStatModifier modifier)
    {
    }

    public abstract void Influence(ICombatant target, IStatusCombatContext context);

    public virtual void RemoveModifier(IStatModifier modifier)
    {
    }
}