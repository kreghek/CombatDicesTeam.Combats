using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.UseCaseTests;

internal class HealTests
{
    [Test]
    public void Effect_heal_combatant_and_throws_event()
    {
        // ARRANGE

        const int MAX_ROUNDS = 1;

        var roundQueueResolverMock = new Mock<IRoundQueueResolver>();
        roundQueueResolverMock
            .Setup(resolver => resolver.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()))
            .Returns<IReadOnlyCollection<ICombatant>>(combatants => new List<ICombatant>(combatants));

        var roundQueueResolver = roundQueueResolverMock.Object;
        var stateStrategy = new LimitedRoundsCombatStateStrategy(new EliminatingCombatStateStrategy(), MAX_ROUNDS);
        var combat = new TestableCombatEngine3(Mock.Of<IDice>(), roundQueueResolver, stateStrategy);

        var combatantStatType = new CombatantStatType("test-stat");
        const int STAT_VALUE = 10;
        var stat = new CombatantStat(combatantStatType, new StatValue(STAT_VALUE));

        var heroMock = new Mock<ICombatant>();
        heroMock.Setup(x => x.IsPlayerControlled).Returns(true);
        heroMock.SetupGet(x => x.Stats).Returns(new[] { stat });
        var heroes = new[] { new FormationSlot(0, 0) { Combatant = heroMock.Object } };

        var monsterMock = new Mock<ICombatant>();
        monsterMock.Setup(x => x.IsPlayerControlled).Returns(false);
        monsterMock.Setup(x => x.AddStatus(It.IsAny<ICombatantStatus>(), It.IsAny<ICombatantStatusImposeContext>(),
                It.IsAny<ICombatantStatusLifetimeImposeContext>()))
            .Callback<ICombatantStatus, ICombatantStatusImposeContext, ICombatantStatusLifetimeImposeContext>(
                (status, _, _) =>
                {
                    status.Impose(monsterMock.Object, new CombatantStatusImposeContext(combat));
                    status.Lifetime.HandleImposed(status,
                        new CombatantStatusLifetimeImposeContext(monsterMock.Object, combat));
                });
        var monsters = new[] { new FormationSlot(0, 0) { Combatant = monsterMock.Object } };

        combat.Initialize(heroes, monsters);

        var combatMovement = new CombatMovementInstance(
            new CombatMovement(new CombatMovementSid("TestCombatMovement"),
                new CombatMovementCost(0),
                CombatMovementEffectConfig.Create(new[]
                {
                    new ChangeCurrentStatEffect(Mock.Of<ITargetSelector>(x=>
                        x.GetMaterialized(It.IsAny<ICombatant>(), It.IsAny<TargetSelectorContext>()) == new []{heroMock.Object}),
                        combatantStatType, 
                        new GenericRange<int>(1, 1))
                })));

        using var monitor = combat.Monitor(); 

        // ACT

        var execution = combat.CreateCombatMovementExecution(combatMovement);

        foreach (var executionEffectImposeItem in execution.EffectImposeItems)
        {
            foreach (var materializedTarget in executionEffectImposeItem.MaterializedTargets)
            {
                executionEffectImposeItem.ImposeDelegate(materializedTarget);
            }
        }

        // ASSERT

        monitor.Should().Raise(nameof(combat.CombatantStatChanged)).WithArgs<CombatantDamagedEventArgs>(x =>
            x.Combatant == heroMock.Object && ReferenceEquals(x.StatType, combatantStatType));
    }
}
