namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public sealed class UntilCombatantEffectMeetPredicatesLifetimeFactory : ICombatantStatusLifetimeFactory
{
    private readonly IReadOnlyCollection<ICombatMovePredicate> _combatMovePredicates;

    public UntilCombatantEffectMeetPredicatesLifetimeFactory(params ICombatMovePredicate[] combatMovePredicates)
    {
        _combatMovePredicates = combatMovePredicates;
    }

    public ICombatantStatusLifetime Create()
    {
        return new UntilCombatantEffectMeetPredicatesLifetime(_combatMovePredicates);
    }
}