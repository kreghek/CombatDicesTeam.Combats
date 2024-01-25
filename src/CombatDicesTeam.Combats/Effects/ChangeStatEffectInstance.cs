using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats.Effects;

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

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        var combatantStatus =
            new ModifyStatCombatantStatus(_combatantStatusSid, Lifetime,
                new CombatMovementCombatantStatusSource(context.Actor), BaseEffect.TargetStatType, BaseEffect.Value);

        context.StatusLifetimeImposedContext.Combat.ImposeCombatantStatus(target, combatantStatus);
    }
}