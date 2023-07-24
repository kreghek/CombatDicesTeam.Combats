namespace Core.Combats;

public interface ITargetSelector
{
    IReadOnlyList<CombatMoveTargetEstimate> GetEstimate(ICombatant actor, ITargetSelectorContext context)
    {
        return GetMaterialized(actor, context)
            .Select(x => new CombatMoveTargetEstimate(x, CombatMoveTargetEstimateType.Exactly))
            .ToArray();
    }

    IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context);
}