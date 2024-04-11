namespace CombatDicesTeam.Combats.Effects;

public abstract class EffectInstanceBase<TEffect> : IEffectInstance where TEffect : IEffect
{
    protected EffectInstanceBase(TEffect baseEffect)
    {
        BaseEffect = baseEffect;
    }

    public TEffect BaseEffect { get; }
    public ITargetSelector Selector => BaseEffect.Selector;

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => BaseEffect.ImposeConditions;

    public virtual void AddModifier(IStatModifier modifier)
    {
    }

    public abstract void Influence(ICombatant target, ICombatMovementContext context);

    public virtual void RemoveModifier(IStatModifier modifier)
    {
    }
}