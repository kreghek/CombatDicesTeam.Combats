using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.Tests;

public sealed class TestableCombatEngine : CombatEngineBase
{
    public TestableCombatEngine(IDice dice, IRoundQueueResolver roundQueueResolver, ICombatStateStrategy stateStrategy)
        : base(dice, roundQueueResolver, stateStrategy)
    {
    }

    public override CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement)
    {
        throw new NotImplementedException();
    }

    protected override bool DetectCombatantIsDead(ICombatant combatant)
    {
        return false;
    }

    protected override void PrepareCombatantsToNextRound()
    {
    }

    protected override void RestoreStatsOnWait()
    {
    }

    protected override void SpendManeuverResources()
    {
    }
}
