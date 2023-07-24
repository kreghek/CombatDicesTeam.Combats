using Core.Combats.CombatantStatuses;

namespace Core.Combats.Effects;

public sealed class ChangeStatEffectInstance : EffectInstanceBase<ChangeStatEffect>
{
    private readonly ICombatantStatusSid _combatantStatusSid;

    public ChangeStatEffectInstance(ICombatantStatusSid combatantEffectSid, ChangeStatEffect baseEffect,
        ICombatantStatusLifetime combatantStatusLifetime) :
        base(baseEffect)
    {
        _combatantStatusSid = combatantEffectSid;
        Lifetime = combatantStatusLifetime;
    }

    public ICombatantStatusLifetime Lifetime { get; }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        var combatantStatus =
            new ChangeStatCombatantStatus(_combatantStatusSid, Lifetime, BaseEffect.TargetStatType, BaseEffect.Value);

        context.StatusLifetimeImposedContext.Combat.ImposeCombatantEffect(target, combatantStatus);
    }
}