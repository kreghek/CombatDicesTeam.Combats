using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class ChangeCurrentStatEffectInstance : EffectInstanceBase<ChangeCurrentStatEffect>
{
    public GenericRange<IStatValue> StatValue { get; }

    public ChangeCurrentStatEffectInstance(ChangeCurrentStatEffect baseEffect) : base(baseEffect)
    {
        StatValue = new GenericRange<IStatValue>(new StatValue(baseEffect.StatValue.Min),
            new StatValue(baseEffect.StatValue.Max));
    }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        var rolledValue = context.Dice.Roll(BaseEffect.StatValue.Min, BaseEffect.StatValue.Max);

        var statValue = target.Stats.Single(x => x.Type == BaseEffect.TargetStatType).Value;
        if (rolledValue > 0)
        {
            statValue.Restore(rolledValue);
        }
        else
        {
            statValue.Consume(rolledValue);
        }

        context.DamageCombatantStat(target, BaseEffect.TargetStatType,
            new CombatEngineBase.StatDamage(rolledValue, rolledValue));
    }
}