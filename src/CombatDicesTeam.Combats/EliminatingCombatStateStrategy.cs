using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

public sealed class LimitedRoundsCombatStateStrategy : ICombatStateStrategy
{
    private readonly ICombatStateStrategy _baseStrategy;
    private readonly int _maxRound;

    public LimitedRoundsCombatStateStrategy(ICombatStateStrategy baseStrategy, int maxRound)
    {
        _baseStrategy = baseStrategy;
        _maxRound = maxRound;
    }
    
    public ICombatState CalculateCurrentState(ICombatStateStrategyContext context)
    {
        if (context.CurrentRound > _maxRound)
        {
            return CommonCombatStates.Draw;
        }

        return _baseStrategy.CalculateCurrentState(context);
    }
}

/// <summary>
/// Strategy returns victory or failure based on combatants alive.
/// </summary>
[PublicAPI]
public sealed class EliminatingCombatStateStrategy: ICombatStateStrategy
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