using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class LifeDrawEffectInstance : EffectInstanceBase<LifeDrawEffect>
{
    public LifeDrawEffectInstance(LifeDrawEffect damageEffect) : base(damageEffect)
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
            Math.Max(rolledDamage - target.Stats.Single(x => x.Type == CombatantStatTypes.Defense).Value.Current, 0);

        //var damageRemains = TakeStat(target, UnitStatType.ShieldPoints, absorbedDamage);

        var damageRemains = context.DamageCombatantStat(target, CombatantStatTypes.ShieldPoints, absorbedDamage);

        if (damageRemains > 0)
        {
            //TakeStat(target, UnitStatType.HitPoints, damageRemains);
            var stolenHitPoints = context.DamageCombatantStat(target, CombatantStatTypes.HitPoints, damageRemains);

            context.Actor.Stats.Single(x => x.Type == CombatantStatTypes.HitPoints).Value.Restore(damageRemains);
        }
    }

    public override void RemoveModifier(IUnitStatModifier modifier)
    {
        Damage.Min.RemoveModifier(modifier);
        Damage.Max.RemoveModifier(modifier);
    }

    private static int TakeStat(ICombatant combatant, ICombatantStatType statType, int value)
    {
        var stat = combatant.Stats.SingleOrDefault(x => x.Type == statType);

        if (stat is null)
        {
            return value;
        }

        var d = Math.Min(value, stat.Value.Current);
        stat.Value.Consume(d);

        var remains = value - d;

        return remains;
    }
}