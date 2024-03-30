using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class UntilCombatantEffectMeetPredicatesLifetime : ICombatantStatusLifetime
{
    private readonly IReadOnlyCollection<ICombatantStatusLifetimeExpirationCondition> _combatantStatusPredicates;

    private ICombatant? _owner;
    private CombatEngineBase? _combat;

    public UntilCombatantEffectMeetPredicatesLifetime(
        IReadOnlyCollection<ICombatantStatusLifetimeExpirationCondition> combatantStatusPredicates)
    {
        _combatantStatusPredicates = combatantStatusPredicates;
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

        if (_combatantStatusPredicates.OfType<IUsedCombatMovementLifetimeExpirationCondition>().All(x => x.Check(_owner, e.Move)))
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
        context.Combat.CombatantHasBeenDamaged += Combat_CombatantChangedState;
        context.Combat.CombatantHasChangePosition += Combat_CombatantChangedState;
        context.Combat.CombatantHasBeenDefeated += Combat_CombatantChangedState;
    }

    private void Combat_CombatantChangedState(object? sender, EventArgs e)
    {
        if (_owner is not null && _combat is not null)
        {
            if (_combatantStatusPredicates.OfType<ICombatantStateLifetimeExpirationCondition>().All(x => x.Check(_owner, _combat)))
            {
                IsExpired = true;
            }
        }
    }

    public void HandleDispelling(ICombatantStatus combatantEffect, ICombatantStatusLifetimeDispelContext context)
    {
        context.Combat.CombatantUsedMove -= Combat_CombatantUsedMove;
        context.Combat.CombatantHasBeenDamaged -= Combat_CombatantChangedState;
        context.Combat.CombatantHasChangePosition -= Combat_CombatantChangedState;
        context.Combat.CombatantHasBeenDefeated -= Combat_CombatantChangedState;
    }
}