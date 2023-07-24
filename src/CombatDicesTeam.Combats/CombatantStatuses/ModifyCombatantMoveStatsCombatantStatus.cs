namespace Core.Combats.CombatantStatuses;

/// <summary>
/// Change combatant's combat moves stat.
/// Not effect, stats. Exmaple - cost.
/// </summary>
//TODO Change target selector.
//TODO Change only in hand or in pull.
public sealed class ModifyCombatantMoveStatsCombatantStatus : CombatantStatusBase
{
    private readonly StatModifier _modifier;
    private readonly CombatantMoveStats _stats;

    public ModifyCombatantMoveStatsCombatantStatus(ICombatantStatusSid sid,
        ICombatantStatusLifetime lifetime,
        CombatantMoveStats stats,
        int value) : base(sid, lifetime)
    {
        _stats = stats;

        _modifier = new StatModifier(value);
    }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        var allCombatMoves = GetAllCombatMoves(combatant);

        foreach (var combatMovementInstance in allCombatMoves)
        {
            switch (_stats)
            {
                case CombatantMoveStats.Cost:
                    combatMovementInstance.Cost.Amount.RemoveModifier(_modifier);
                    break;
            }
        }
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        base.Impose(combatant, combatantEffectImposeContext);

        var allCombatMoves = GetAllCombatMoves(combatant);

        foreach (var combatMovementInstance in allCombatMoves)
        {
            switch (_stats)
            {
                case CombatantMoveStats.Cost:
                    combatMovementInstance.Cost.Amount.AddModifier(_modifier);
                    break;
            }
        }
    }

    private static IEnumerable<CombatMovementInstance> GetAllCombatMoves(ICombatant combatant)
    {
        return combatant.CombatMovementContainers.SelectMany(x=>x.GetItems()).Where(x => x is not null).Select(x => x!);
    }
}