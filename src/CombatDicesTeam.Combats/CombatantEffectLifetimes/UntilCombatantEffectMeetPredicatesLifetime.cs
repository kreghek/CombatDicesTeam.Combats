using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class UntilCombatantEffectMeetPredicatesLifetime : ICombatantStatusLifetime
{
    private readonly IReadOnlyCollection<ICombatantStatusLifetimeExpirationCondition>
        _combatantStatusLifetimeExpirationCondition;

    private CombatEngineBase? _combat;

    private ICombatant? _owner;

    public UntilCombatantEffectMeetPredicatesLifetime(
        IReadOnlyCollection<ICombatantStatusLifetimeExpirationCondition> combatantStatusLifetimeExpirationCondition)
    {
        _combatantStatusLifetimeExpirationCondition = combatantStatusLifetimeExpirationCondition;
    }

    private void Combat_CombatantChangedState(object? sender, EventArgs e)
    {
        if (_owner is null || _combat is null)
        {
            //TODO Handle this as error
            return;
        }

        if (_combatantStatusLifetimeExpirationCondition.OfType<ICombatantStateLifetimeExpirationCondition>()
            .All(x => x.Check(_owner, _combat)))
        {
            IsExpired = true;
        }
    }

    private void Combat_CombatantUsedMove(object? sender, CombatantHandChangedEventArgs e)
    {
        if (_owner != e.Combatant)
        {
            // Check only if owner performs combat movements.
            return;
        }

        if (_combat is null)
        {
            return;
        }

        if (_combatantStatusLifetimeExpirationCondition.OfType<IUsedCombatMovementLifetimeExpirationCondition>()
            .All(x => x.Check(_owner, e.Move)))
        {
            IsExpired = true;
        }
    }

    public bool IsExpired { get; private set; }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
    }

    public void HandleImposed(ICombatantStatus combatantEffect, ICombatantStatusLifetimeImposeContext context)
    {
        _owner = context.TargetCombatant;
        _combat = context.Combat;

        context.Combat.CombatantUsedMove += Combat_CombatantUsedMove;
        context.Combat.CombatantStatChanged += Combat_CombatantChangedState;
        context.Combat.CombatantHasChangePosition += Combat_CombatantChangedState;
        context.Combat.CombatantHasBeenDefeated += Combat_CombatantChangedState;
    }

    public void HandleDispelling(ICombatantStatus combatantEffect, ICombatantStatusLifetimeDispelContext context)
    {
        context.Combat.CombatantUsedMove -= Combat_CombatantUsedMove;
        context.Combat.CombatantStatChanged -= Combat_CombatantChangedState;
        context.Combat.CombatantHasChangePosition -= Combat_CombatantChangedState;
        context.Combat.CombatantHasBeenDefeated -= Combat_CombatantChangedState;
    }
}