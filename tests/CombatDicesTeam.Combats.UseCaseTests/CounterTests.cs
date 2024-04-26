using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.UseCaseTests;

public class CounterTests
{
    [Test]
    public void Counter_adds_status_to_attacked_combatant()
    {
        // ARRANGE

        const int MAX_ROUNDS = 1;

        var roundQueueResolverMock = new Mock<IRoundQueueResolver>();
        roundQueueResolverMock
            .Setup(resolver => resolver.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()))
            .Returns<IReadOnlyCollection<ICombatant>>(combatants => new List<ICombatant>(combatants));

        var roundQueueResolver = roundQueueResolverMock.Object;
        var stateStrategy = new LimitedRoundsCombatStateStrategy(new EliminatingCombatStateStrategy(), MAX_ROUNDS);
        var combat = new TestableCombatEngine(Mock.Of<IDice>(), roundQueueResolver, stateStrategy);

        var heroMock = new Mock<ICombatant>();
        heroMock.Setup(x => x.IsPlayerControlled).Returns(true);
        heroMock.Setup(x => x.AddStatus(It.IsAny<ICombatantStatus>(), It.IsAny<ICombatantStatusImposeContext>(),
                It.IsAny<ICombatantStatusLifetimeImposeContext>()))
            .Callback<ICombatantStatus, ICombatantStatusImposeContext, ICombatantStatusLifetimeImposeContext>(
                (status, _, _) =>
                {
                    status.Impose(heroMock.Object, new CombatantStatusImposeContext(combat));
                    status.Lifetime.HandleImposed(status,
                        new CombatantStatusLifetimeImposeContext(heroMock.Object, combat));
                });
        var statType = Mock.Of<ICombatantStatType>();
        heroMock.Setup(x => x.Stats).Returns(new[]
        {
            Mock.Of<ICombatantStat>(cs => cs.Type == statType && cs.Value == Mock.Of<IStatValue>(v => v.Current == 1))
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

        var resultStatusSid = Mock.Of<ICombatantStatusSid>();
        var resultStatusFactory =
            new CombatStatusFactory(_ => Mock.Of<ICombatantStatus>(s =>
                s.Sid == resultStatusSid && s.Lifetime == new OwnerBoundCombatantEffectLifetime()));

        var counterStatusFactory = new CombatStatusFactory(source =>
            new CounterCombatantStatus(
                Mock.Of<ICombatantStatusSid>(),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                new AddStatusCounterReaction(resultStatusFactory, new EnemiesAuraTargetSelector())));

        var counterStatus = counterStatusFactory.Create(Mock.Of<ICombatantStatusSource>());

        counterStatus.Impose(heroMock.Object, new CombatantStatusImposeContext(combat));

        // ACT

        combat.DamageCombatant(heroMock.Object, monsterMock.Object, statType, 1);

        // ASSERT

        monsterMock.Verify(x => x.AddStatus(It.Is<ICombatantStatus>(s => s.Sid == resultStatusSid),
            It.IsAny<ICombatantStatusImposeContext>(), It.IsAny<ICombatantStatusLifetimeImposeContext>()));
    }
}