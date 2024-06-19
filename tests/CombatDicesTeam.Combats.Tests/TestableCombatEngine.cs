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

    public int TestHandleCombatantDamagedToStat(ICombatant combatant, IStatChangingSource damageSource,
        ICombatantStatType statType, StatDamage damage)
    {
        return HandleCombatantDamagedToStat(combatant, damageSource, statType, damage);
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
