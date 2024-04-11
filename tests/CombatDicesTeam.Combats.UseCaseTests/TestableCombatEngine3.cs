using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.UseCaseTests;

public sealed class TestableCombatEngine3 : CombatEngineBase
{
    public TestableCombatEngine3(IDice dice, IRoundQueueResolver roundQueueResolver, ICombatStateStrategy stateStrategy)
        : base(dice, roundQueueResolver, stateStrategy)
    {
    }

    public override CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement)
    {
        var statusContext =
            new StatusCombatContext(CurrentCombatant, Field, Dice, HandleCombatantDamagedToStat,
                HandleSwapFieldPositions, this);

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effectInstance in movement.Effects)
        {
            if (!effectInstance.ImposeConditions.All(x => x.Check(CurrentCombatant, Field)))
            {
                // It is not meet the conditions.
                // Ignore this effect.
                continue;
            }

            var effectInstanceClosure = effectInstance;

            void EffectInfluenceDelegate(ICombatant materializedTarget)
            {
                effectInstanceClosure.Influence(materializedTarget, statusContext);
            }

            var effectTargets = effectInstance.Selector.GetMaterialized(CurrentCombatant, GetCurrentSelectorContext());

            var effectImposeItem = new CombatEffectImposeItem(EffectInfluenceDelegate, effectTargets);

            effectImposeItems.Add(effectImposeItem);
        }

        void CompleteSkillAction()
        {
        }

        var movementExecution = new CombatMovementExecution(CompleteSkillAction, effectImposeItems);

        return movementExecution;
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