using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class DamageEffectInstance : EffectInstanceBase<DamageEffect>
{
    private readonly DamageEffectConfig _damageEffectConfig;

    public DamageEffectInstance(DamageEffect damageEffect, DamageEffectConfig damageEffectConfig) : base(damageEffect)
    {
        _damageEffectConfig = damageEffectConfig;
        Damage = new GenericRange<IStatValue>(new StatValue(damageEffect.Damage.Min),
            new StatValue(damageEffect.Damage.Max));
    }

    public GenericRange<IStatValue> Damage { get; }

    public override void AddModifier(IStatModifier modifier)
    {
        Damage.Min.AddModifier(modifier);
        Damage.Max.AddModifier(modifier);
    }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        var rolledDamage = context.Dice.Roll(Damage.Min.ActualMax, Damage.Max.ActualMax);

        var absorptionStat = target.Stats.SingleOrDefault(x => x.Type == _damageEffectConfig.AbsorptionStatType);
        var currentAbsorptionValue = absorptionStat?.Value.Current;
        var absorbedDamage = Math.Max(rolledDamage - currentAbsorptionValue.GetValueOrDefault(), 0);

        var damageRemains = context.DamageCombatantStat(target, _damageEffectConfig.ProtectionStatType,
            new StatDamage(absorbedDamage, rolledDamage));

        if (BaseEffect.DamageType == DamageType.ProtectionOnly)
        {
            return;
        }

        if (damageRemains > 0)
        {
            context.DamageCombatantStat(target, _damageEffectConfig.MainStatType,
                new StatDamage(damageRemains, rolledDamage));
        }
    }

    public override void RemoveModifier(IStatModifier modifier)
    {
        Damage.Min.RemoveModifier(modifier);
        Damage.Max.RemoveModifier(modifier);
    }
}