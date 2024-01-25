using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats.Effects;

public sealed class ModifyEffectsEffectInstance : EffectInstanceBase<ModifyEffectsEffect>
{
    private readonly ICombatantStatusSid _imposedStatusSid;

    public ModifyEffectsEffectInstance(ICombatantStatusSid imposedStatusSid, ModifyEffectsEffect baseEffect) :
        base(baseEffect)
    {
        _imposedStatusSid = imposedStatusSid;

        BuffPower = new StatValue(BaseEffect.Value);
        Duration = new StatValue(1);
    }

    public IStatValue BuffPower { get; }

    public IStatValue Duration { get; }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        context.StatusImposedContext.ImposeCombatantStatus(target,
            new CombatMovementCombatantStatusSource(context.Actor),
            new ModifyEffectsCombatantStatusFactory(_imposedStatusSid,
                new MultipleCombatantTurnEffectLifetimeFactory(Duration.ActualMax),
                BuffPower.ActualMax));
    }
}