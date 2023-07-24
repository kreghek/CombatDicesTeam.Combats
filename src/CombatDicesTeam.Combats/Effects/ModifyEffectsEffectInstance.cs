using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats.Effects;

public sealed class ModifyEffectsEffectInstance : EffectInstanceBase<ModifyEffectsEffect>
{
    private readonly ICombatantStatusSid _imposedStatusSid;

    public ModifyEffectsEffectInstance(ICombatantStatusSid imposedStatusSid, ModifyEffectsEffect baseEffect) : base(baseEffect)
    {
        _imposedStatusSid = imposedStatusSid;
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        context.StatusImposedContext.ImposeCombatantStatus(target,
            new ModifyEffectsCombatantStatusFactory(_imposedStatusSid,
                new MultipleCombatantTurnEffectLifetimeFactory(1),
                BaseEffect.Value));
    }
}