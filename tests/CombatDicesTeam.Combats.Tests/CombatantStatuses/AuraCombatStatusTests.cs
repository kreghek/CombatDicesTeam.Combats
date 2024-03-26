using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.Tests.CombatantStatuses;

public class AuraCombatStatusTests
{
    [Test]
    public void Impose_adds_aura_status_to_current_selected_targets()
    {
        // ARRANGE

        var auraStatus = Mock.Of<ICombatantStatus>();

        var factoryMock = new Mock<ICombatantStatusFactory>();
        factoryMock.Setup(x => x.Create(It.IsAny<ICombatantStatusSource>())).Returns(auraStatus);

        var targetCombatantMock = new Mock<ICombatant>();
        var targetCombatant = targetCombatantMock.Object;

        var sut = new AuraCombatantStatus(Mock.Of<ICombatantStatusSid>(), Mock.Of<ICombatantStatusLifetime>(),
            Mock.Of<ICombatantStatusSource>(), _ => factoryMock.Object,
            new EnemiesAuraTargetSelector());

        var auraOwnerCombatant = Mock.Of<ICombatant>(x => x.IsPlayerControlled == true);

        var combat = new TestCombat(Mock.Of<IDice>(), Mock.Of<IRoundQueueResolver>(x =>
            x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()) == new[]
            {
                auraOwnerCombatant,
                targetCombatant
            }), Mock.Of<ICombatStateStrategy>());

        combat.Initialize(new[] { new FormationSlot(0, 0) { Combatant = auraOwnerCombatant } },
            new[] { new FormationSlot(0, 0) { Combatant = targetCombatant } });

        // ACT

        sut.Impose(auraOwnerCombatant, Mock.Of<ICombatantStatusImposeContext>(x => x.Combat == combat));

        // ASSERT

        targetCombatantMock.Verify(x => x.AddStatus(It.Is<ICombatantStatus>(c => c == auraStatus),
            It.IsAny<ICombatantStatusImposeContext>(), It.IsAny<ICombatantStatusLifetimeImposeContext>()));
    }

    private sealed class TestCombat : CombatEngineBase
    {
        public TestCombat(IDice dice, IRoundQueueResolver roundQueueResolver, ICombatStateStrategy stateStrategy) :
            base(dice, roundQueueResolver, stateStrategy)
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