namespace CombatDicesTeam.Combats.Effects;

public sealed class InterruptEffectInstance : EffectInstanceBase<InterruptEffect>
{
    public InterruptEffectInstance(InterruptEffect stunEffect) : base(stunEffect)
    {
    }

    public override void AddModifier(IStatModifier modifier)
    {
    }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        context.PassTurn(target);
    }

    public override void RemoveModifier(IStatModifier modifier)
    {
    }
}