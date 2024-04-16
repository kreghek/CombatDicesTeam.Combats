using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.Tests.CombatantEffectLifetimes;

public class TargetCombatantsBoundCombatantStatusLifetimeTests
{
    [Test]
    public void IsExpired_sets_true_when_every_bound_combatants_are_defeated()
    {
        // ARRANGE

        var boundCombatant = Mock.Of<ICombatant>(x => x.IsPlayerControlled == true);
        var targetCombatant = Mock.Of<ICombatant>(x => x.IsPlayerControlled == false);

        var status = Mock.Of<ICombatantStatus>();

        var combat = new TestCombat(Mock.Of<IDice>(), Mock.Of<IRoundQueueResolver>(), Mock.Of<ICombatStateStrategy>());

        var sut = new TargetCombatantsBoundCombatantStatusLifetime(boundCombatant);

        // ACT

        sut.HandleImposed(status,
            Mock.Of<ICombatantStatusLifetimeImposeContext>(x =>
                x.TargetCombatant == targetCombatant && x.Combat == combat));

        combat.DefeatCombatant(boundCombatant);

        // ASSERT

        sut.IsExpired.Should().BeTrue();
    }

    private sealed class TestCombat : CombatEngineBase
    {
        public TestCombat(IDice dice, IRoundQueueResolver roundQueueResolver, ICombatStateStrategy stateStrategy) :
            base(dice, roundQueueResolver, stateStrategy)
        {
        }

        public void DefeatCombatant(ICombatant combatant)
        {
            DoCombatantHasBeenDefeated(combatant);
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
}