using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class IsAttackCombatMovePredicate : ICombatMovePredicate
{
    public bool Check(ICombatant statusOwner, CombatMovementInstance combatMove)
    {
        return combatMove.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack);
    }
}