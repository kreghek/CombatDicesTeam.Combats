namespace CombatDicesTeam.Combats.Effects;

public sealed class GroupEffectInstance : EffectInstanceBase<GroupEffect>
{
    private readonly IEffectInstance[] _concreteEffects;

    public GroupEffectInstance(GroupEffect baseEffect, IEffectInstance[] concreteEffects) : base(baseEffect)
    {
        _concreteEffects = concreteEffects;
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        foreach (var effect in _concreteEffects)
        {
            effect.Influence(target, context);
        }
    }
}