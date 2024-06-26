using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.Effects;

public sealed class ChangeCurrentStatEffectInstance : EffectInstanceBase<ChangeCurrentStatEffect>
{
    public ChangeCurrentStatEffectInstance(ChangeCurrentStatEffect baseEffect) : base(baseEffect)
    {
        StatValue = new GenericRange<IStatValue>(new StatValue(baseEffect.StatValue.Min),
            new StatValue(baseEffect.StatValue.Max));
    }

    [PublicAPI]
    public GenericRange<IStatValue> StatValue { get; }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        var rolledValue = context.Dice.Roll(StatValue.Min.ActualMax, StatValue.Max.ActualMax);

        context.ChangeCombatStat(target, new CombatantMovementStatChangingSource(context.Actor),
            BaseEffect.TargetStatType, rolledValue);
    }
}