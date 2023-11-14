using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

/// <summary>
/// Strategy returns Draw if combat is too long.
/// </summary>
[PublicAPI]
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
        return context.CurrentRound > _maxRound
            ? CommonCombatStates.Draw
            : _baseStrategy.CalculateCurrentState(context);
    }
}