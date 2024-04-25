using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.UseCaseTests;

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

    public void DamageCombatant(ICombatant target, ICombatant actor, ICombatantStatType statTypeToDamage, int damageAmount)
    {
        HandleCombatantDamagedToStat(target, new CombatantDamageSource(actor), statTypeToDamage, damageAmount);
    }

    public void DefeatCombatant(ICombatant target)
    {
        DoCombatantHasBeenDefeated(target);
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
