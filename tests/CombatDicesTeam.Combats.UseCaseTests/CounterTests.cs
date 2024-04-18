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

        ICombatantStatus? imposedAuraStatusEffect = null;
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

                    imposedAuraStatusEffect = status;
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

        var aura = new CombatStatusFactory(source =>
            new AuraCombatantStatus(new CombatantStatusSid("test-aura"),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                owner => new CombatStatusFactory(_ =>
                    Mock.Of<ICombatantStatus>(s =>
#pragma warning disable CS0252, CS0253
                        s.Sid == new CombatantStatusSid("test-status-aura") &&
#pragma warning restore CS0252, CS0253
                        s.Lifetime == new TargetCombatantsBoundCombatantStatusLifetime(owner))
                ),
                new EnemiesAuraTargetSelector()
            ));

        var auraStatus = aura.Create(Mock.Of<ICombatantStatusSource>());

        // ACT

        auraStatus.Impose(combat.CurrentCombatants.Single(x => x == monsterMock.Object),
            new CombatantStatusImposeContext(combat));

        // ASSERT

        heroMock.Verify(x => x.AddStatus(It.Is<ICombatantStatus>(s => s.Sid.ToString() == "test-status-aura"),
            It.IsAny<ICombatantStatusImposeContext>(), It.IsAny<ICombatantStatusLifetimeImposeContext>()));

        // ACT

        combat.DefeatCombatant(monsterMock.Object);

        // ASSERT
        imposedAuraStatusEffect?.Lifetime.IsExpired.Should().BeTrue();
    }
}