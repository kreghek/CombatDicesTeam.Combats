using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.UseCaseTests;

public sealed class TestableCombatEngine2 : CombatEngineBase
{
    public TestableCombatEngine2(IDice dice, IRoundQueueResolver roundQueueResolver, ICombatStateStrategy stateStrategy)
        : base(dice, roundQueueResolver, stateStrategy)
    {
    }

    public override CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement)
    {
        var statusContext =
            new CombatMovementContext(CurrentCombatant, Field, Dice, this);

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effectInstance in movement.Effects)
        {
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
