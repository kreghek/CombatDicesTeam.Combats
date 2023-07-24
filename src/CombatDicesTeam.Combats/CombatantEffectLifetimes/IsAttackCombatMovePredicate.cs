namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public sealed class IsAttackCombatMovePredicate : ICombatMovePredicate
{
    public bool Check(CombatMovementInstance combatMove)
    {
        return combatMove.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack);
    }
}