namespace Core.Combats.CombatantEffectLifetimes;

public sealed class UntilCombatantEffectMeetPredicatesLifetime : ICombatantStatusLifetime
{
    private readonly IReadOnlyCollection<ICombatMovePredicate> _combatMovePredicates;
    private ICombatant? _ownedCombatant;

    public UntilCombatantEffectMeetPredicatesLifetime(IReadOnlyCollection<ICombatMovePredicate> combatMovePredicates)
    {
        _combatMovePredicates = combatMovePredicates;
    }

    private void Combat_CombatantUsedMove(object? sender, CombatantHandChangedEventArgs e)
    {
        if (_ownedCombatant != e.Combatant)
        {
            // Check only if owner performs combat movements.
            return;
        }

        if (_combatMovePredicates.All(x => x.Check(e.Move)))
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
        _ownedCombatant = context.TargetCombatant;
        context.Combat.CombatantUsedMove += Combat_CombatantUsedMove;
    }

    public void HandleDispelling(ICombatantStatus combatantEffect, ICombatantStatusLifetimeDispelContext context)
    {
        context.Combat.CombatantUsedMove -= Combat_CombatantUsedMove;
    }
}