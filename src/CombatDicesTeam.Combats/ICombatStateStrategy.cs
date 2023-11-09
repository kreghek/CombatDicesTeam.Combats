namespace CombatDicesTeam.Combats;

public interface ICombatStateStrategy
{
    ICombatState CalculateCurrentState(CombatEngineBase combat);
}

public sealed class EliminatingCombatStateStrategy: ICombatStateStrategy
{
    public ICombatState CalculateCurrentState(CombatEngineBase combat)
    {
        var aliveUnits = combat.CurrentCombatants.Where(x => !x.IsDead).ToArray();
        var playerUnits = aliveUnits.Where(x => x.IsPlayerControlled);
        var hasPlayerUnits = playerUnits.Any();

        var cpuUnits = aliveUnits.Where(x => !x.IsPlayerControlled);
        var hasCpuUnits = cpuUnits.Any();

        // TODO Looks like XOR
        if (hasPlayerUnits && !hasCpuUnits)
        {
            return true;
        }

        if (!hasPlayerUnits && hasCpuUnits)
        {
            return true;
        }

        return false;
    }
}

public interface ICombatState
{
    bool IsFinalState { get; }
}

public class CombatState : ICombatState
{
    public CombatState(bool isFinalState = true)
    {
        IsFinalState = isFinalState;
    }

    public bool IsFinalState { get; }
}

public static class CommonCombatStates
{
    public static ICombatState InProgress { get; } = new CombatState(false);
    public static ICombatState Victory { get; } = new CombatState();
    public static ICombatState Failure { get; } = new CombatState();
    public static ICombatState Draw { get; } = new CombatState();
}