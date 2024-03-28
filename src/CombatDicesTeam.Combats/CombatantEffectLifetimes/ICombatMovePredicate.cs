namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public interface ICombatantStatusPredicate
{
}

public interface ICombatMovePredicate: ICombatantStatusPredicate
{
    bool Check(CombatMovementInstance combatMove, ICombatant statusOwner);
}

public interface ICombatantStateChangedPredicate: ICombatantStatusPredicate
{
    bool Check(ICombatant statusOwner, CombatEngineBase combatEngine);
}