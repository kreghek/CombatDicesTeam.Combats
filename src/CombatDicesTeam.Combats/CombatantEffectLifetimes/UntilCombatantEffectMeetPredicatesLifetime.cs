using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class UntilCombatantEffectMeetPredicatesLifetime : ICombatantStatusLifetime
{
    private readonly IReadOnlyCollection<ICombatantStatusPredicate> _combatantStatusPredicates;

    private ICombatant? _owner;
    private CombatEngineBase? _combat;

    public UntilCombatantEffectMeetPredicatesLifetime(
        IReadOnlyCollection<ICombatantStatusPredicate> combatantStatusPredicates)
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

        if (_combatantStatusPredicates.OfType<ICombatMovePredicate>().All(x => x.Check(e.Move, _owner)))
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
            if (_combatantStatusPredicates.OfType<ICombatantStateChangedPredicate>().All(x => x.Check(_owner, _combat)))
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