using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class DamageEffectInstance : EffectInstanceBase<DamageEffect>
{
    public DamageEffectInstance(DamageEffect damageEffect) : base(damageEffect)
    {
        Damage = new GenericRange<IStatValue>(new StatValue(damageEffect.Damage.Min), new StatValue(damageEffect.Damage.Max));
    }

    public GenericRange<IStatValue> Damage { get; }

    public override void AddModifier(IUnitStatModifier modifier)
    {
        Damage.Min.AddModifier(modifier);
        Damage.Max.AddModifier(modifier);
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        var rolledDamage = context.Dice.Roll(Damage.Min.ActualMax, Damage.Max.ActualMax);

        var absorbedDamage =
            Math.Max(rolledDamage - target.Stats.Single(x => x.Type == CombatantStatTypes.Defense).Value.ActualMax, 0);

        var damageRemains = context.DamageCombatantStat(target, CombatantStatTypes.ShieldPoints, absorbedDamage);

        if (BaseEffect.DamageType == DamageType.ShieldsOnly)
        {
            return;
        }

        if (damageRemains > 0)
        {
            context.DamageCombatantStat(target, CombatantStatTypes.HitPoints, damageRemains);
        }
    }

    public override void RemoveModifier(IUnitStatModifier modifier)
    {
        Damage.Min.RemoveModifier(modifier);
        Damage.Max.RemoveModifier(modifier);
    }
}