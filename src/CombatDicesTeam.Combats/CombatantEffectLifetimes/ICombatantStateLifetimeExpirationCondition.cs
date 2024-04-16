namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

/// <summary>
/// Predicate to determine when conditions for litetime status to expire are met.
/// Used to monitor combatant state like HP or position.
/// </summary>
/// <remarks>
/// Example, to expire status when a status owner has 1 HP.
/// </remarks>
public interface ICombatantStateLifetimeExpirationCondition : ICombatantStatusLifetimeExpirationCondition
{
    bool Check(ICombatant statusOwner, CombatEngineBase combatEngine);
}