namespace Core.Combats.CombatantEffectLifetimes;

public interface ICombatMovePredicate
{
    bool Check(CombatMovementInstance combatMove);
}