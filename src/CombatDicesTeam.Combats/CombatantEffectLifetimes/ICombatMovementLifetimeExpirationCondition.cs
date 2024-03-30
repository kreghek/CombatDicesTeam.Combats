namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

/// <summary>
/// Predicate to determine when conditions for litetime status to expire are met.
/// Used to monitor used combat movement.
/// </summary>
/// <remarks>
/// Example, to expire status when attack skill used.
/// </remarks>
public interface IUsedCombatMovementLifetimeExpirationCondition : ICombatantStatusLifetimeExpirationCondition
{
    bool Check(ICombatant statusOwner, CombatMovementInstance combatMovement);
}
