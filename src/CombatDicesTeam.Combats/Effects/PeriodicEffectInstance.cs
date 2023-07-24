namespace CombatDicesTeam.Combats.Effects;

public sealed class PeriodicEffectInstance : EffectInstanceBase<PeriodicEffect>
{
    public PeriodicEffectInstance(PeriodicEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        throw new NotImplementedException();
    }
}