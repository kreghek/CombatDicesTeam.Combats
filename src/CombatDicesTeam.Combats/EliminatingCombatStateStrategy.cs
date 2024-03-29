using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

/// <summary>
/// Strategy returns victory or failure based on combatants alive.
/// </summary>
[PublicAPI]
public sealed class EliminatingCombatStateStrategy : ICombatStateStrategy
{
    /// <inheritdoc />
    public ICombatState CalculateCurrentState(ICombatStateStrategyContext context)
    {
        var aliveUnits = context.Combatants.Where(x => !x.IsDead).ToArray();
        var playerUnits = aliveUnits.Where(x => x.IsPlayerControlled);
        var hasPlayerUnits = playerUnits.Any();

        var cpuUnits = aliveUnits.Where(x => !x.IsPlayerControlled);
        var hasCpuUnits = cpuUnits.Any();

        if (hasPlayerUnits && !hasCpuUnits)
        {
            return CommonCombatStates.Victory;
        }

        if (!hasPlayerUnits && hasCpuUnits)
        {
            return CommonCombatStates.Failure;
        }

        return CommonCombatStates.InProgress;
    }
}