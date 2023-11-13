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
            return CommonCombatStates.Victory;
        }

        if (!hasPlayerUnits && hasCpuUnits)
        {
            return CommonCombatStates.Failure;
        }

        return CommonCombatStates.InProgress;
    }
}