namespace CombatDicesTeam.Combats.Effects;

public sealed class InterruptEffectInstance : EffectInstanceBase<InterruptEffect>
{
    public InterruptEffectInstance(InterruptEffect stunEffect) : base(stunEffect)
    {
    }

    public override void AddModifier(IUnitStatModifier modifier)
    {
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        context.PassTurn(target);
    }

    public override void RemoveModifier(IUnitStatModifier modifier)
    {
    }
}