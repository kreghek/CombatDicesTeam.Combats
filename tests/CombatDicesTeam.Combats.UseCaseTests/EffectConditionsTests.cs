using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.UseCaseTests;

internal class EffectConditionsTests
{
    [Test]
    public void Effect_do_not_imposed_then_condition_is_not_satisfied()
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

        var statuses = new List<ICombatantStatus>();

        var heroMock = new Mock<ICombatant>();
        heroMock.Setup(x => x.IsPlayerControlled).Returns(true);
        heroMock.SetupGet(x => x.Stats).Returns(new[] { stat });
        heroMock.Setup(x => x.AddStatus(It.IsAny<ICombatantStatus>(),
                It.IsAny<ICombatantStatusImposeContext>(), It.IsAny<ICombatantStatusLifetimeImposeContext>()))
            .Callback<ICombatantStatus, ICombatantStatusImposeContext, ICombatantStatusLifetimeImposeContext>((status,
                context, _) =>
            {
                status.Impose(heroMock.Object, context);

                statuses.Add(status);
            });
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

        var statusSid = new CombatantStatusSid("PowerUpByStat");
        var statusFactory = new CombatStatusFactory(source =>
        {
            return new TestCombatantStatus(
                statusSid,
                new OwnerBoundCombatantEffectLifetime(),
                source);
        });

        var targetSelector = Mock.Of<ITargetSelector>(x =>
            x.GetMaterialized(It.IsAny<ICombatant>(), It.IsAny<ITargetSelectorContext>()) == new[]
            {
                heroMock.Object
            });

        var combatMovement = new CombatMovementInstance(
            new CombatMovement(new CombatMovementSid("TestCombatMovement"),
                new CombatMovementCost(0),
                CombatMovementEffectConfig.Create(new[]
                {
                    new AddCombatantStatusEffect(targetSelector, statusFactory)
                    {
                        ImposeConditions = new[]
                        {
                            Mock.Of<IEffectCondition>(x =>
                                x.Check(It.IsAny<ICombatant>(), It.IsAny<CombatField>()) == false)
                        }
                    }
                })));

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

        statuses.Should().NotContain(x => x.Sid.Equals(statusSid));
    }

    [Test]
    public void Effect_imposed_then_condition_is_satisfied()
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

        var statuses = new List<ICombatantStatus>();

        var heroMock = new Mock<ICombatant>();
        heroMock.Setup(x => x.IsPlayerControlled).Returns(true);
        heroMock.SetupGet(x => x.Stats).Returns(new[] { stat });
        heroMock.Setup(x => x.AddStatus(It.IsAny<ICombatantStatus>(),
                It.IsAny<ICombatantStatusImposeContext>(), It.IsAny<ICombatantStatusLifetimeImposeContext>()))
            .Callback<ICombatantStatus, ICombatantStatusImposeContext, ICombatantStatusLifetimeImposeContext>((status,
                context, _) =>
            {
                status.Impose(heroMock.Object, context);

                statuses.Add(status);
            });
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

        var statusSid = new CombatantStatusSid("PowerUpByStat");
        var statusFactory = new CombatStatusFactory(source =>
        {
            return new TestCombatantStatus(
                statusSid,
                new OwnerBoundCombatantEffectLifetime(),
                source);
        });

        var targetSelector = Mock.Of<ITargetSelector>(x =>
            x.GetMaterialized(It.IsAny<ICombatant>(), It.IsAny<ITargetSelectorContext>()) == new[]
            {
                heroMock.Object
            });

        var combatMovement = new CombatMovementInstance(
            new CombatMovement(new CombatMovementSid("TestCombatMovement"),
                new CombatMovementCost(0),
                CombatMovementEffectConfig.Create(new[]
                {
                    new AddCombatantStatusEffect(targetSelector, statusFactory)
                    {
                        ImposeConditions = new[]
                        {
                            Mock.Of<IEffectCondition>(x =>
                                x.Check(It.IsAny<ICombatant>(), It.IsAny<CombatField>()) == true)
                        }
                    }
                })));

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

        statuses.Should().Contain(x => x.Sid.Equals(statusSid));
    }

    private sealed class TestCombatantStatus : CombatantStatusBase
    {
        public TestCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
            ICombatantStatusSource source) :
            base(sid, lifetime, source)
        {
        }
    }
}
