using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class IsAttackCombatMovePredicate : ICombatMovePredicate
{
    public bool Check(CombatMovementInstance combatMove, ICombatant statusOwner)
    {
        return combatMove.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack);
    }
}