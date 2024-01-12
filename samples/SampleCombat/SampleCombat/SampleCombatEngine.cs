using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

namespace SampleCombat;

public class SampleCombatEngine: CombatEngineBase
{
    public SampleCombatEngine(IDice dice, IRoundQueueResolver roundQueueResolver, ICombatStateStrategy stateStrategy) : base(dice, roundQueueResolver, stateStrategy)
    {
    }

    public override CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement)
    {
        throw new NotImplementedException();
    }

    protected override bool DetectCombatantIsDead(ICombatant combatant)
    {
        throw new NotImplementedException();
    }

    protected override void PrepareCombatantsToNextRound()
    {
        throw new NotImplementedException();
    }

    protected override void RestoreStatsOnWait()
    {
        throw new NotImplementedException();
    }

    protected override void SpendManeuverResources()
    {
        throw new NotImplementedException();
    }
}