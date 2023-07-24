namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public interface ICombatMovePredicate
{
    bool Check(CombatMovementInstance combatMove);
}