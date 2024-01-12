using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.UseCaseTests;

public class CombatantStatusSourceTests
{
    [Test]
    public void Can_use_actor_effects_count_to_calculate_buff_value()
    {
        // ARRANGE

        const int MAX_ROUNDS = 1;

        var roundQueueResolverMock = new Mock<IRoundQueueResolver>();
        roundQueueResolverMock
            .Setup(resolver => resolver.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()))
            .Returns<IReadOnlyCollection<ICombatant>>(combatants => new List<ICombatant>(combatants));

        var roundQueueResolver = roundQueueResolverMock.Object;
        var stateStrategy = new LimitedRoundsCombatStateStrategy(new EliminatingCombatStateStrategy(), MAX_ROUNDS);
        var combat = new TestableCombatEngine2(Mock.Of<IDice>(), roundQueueResolver, stateStrategy);

        var statuses = new List<ICombatantStatus>();
        var combatantStatType = new CombatantStatType("test-stat");
        var stat = new CombatantStat(combatantStatType, new StatValue(10));
        var heroCombatantMock = new Mock<ICombatant>();
        heroCombatantMock.SetupGet(x => x.IsPlayerControlled).Returns(true);
        heroCombatantMock.SetupGet(x => x.Stats).Returns(new[] { stat });
        heroCombatantMock.Setup(x => x.AddStatus(It.IsAny<ICombatantStatus>(),
                It.IsAny<ICombatantStatusImposeContext>(), It.IsAny<ICombatantStatusLifetimeImposeContext>()))
            .Callback<ICombatantStatus, ICombatantStatusImposeContext, ICombatantStatusLifetimeImposeContext>((status,
                context, _) =>
            {
                status.Impose(heroCombatantMock.Object, context);
                
                statuses.Add(status);
            });
        var heroCombatant = heroCombatantMock.Object;
        var heroes = new[]
        {
            new FormationSlot(0, 0)
            {
                Combatant = heroCombatant
            }
        };

        var monsters = new[]
            { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>(c => c.IsPlayerControlled == false) } };

        combat.Initialize(heroes, monsters);


        var statusFactory = new DelegateCombatStatusFactory((source) => new TestCombatantStatus(
            new CombatantStatusSid("1"), new OwnerBoundCombatantEffectLifetime(), source,
            combatant => combatant.Stats.Single(x => ReferenceEquals(x.Type, combatantStatType)).Value.Current));

        var targetSelector = Mock.Of<ITargetSelector>(x=>x.GetMaterialized(It.IsAny<ICombatant>(), It.IsAny<ITargetSelectorContext>()) == new[]
        {
            heroCombatant
        });

        var combatMovement = new CombatMovementInstance(
            new CombatMovement(new CombatMovementSid("1"),
                new CombatMovementCost(0),
                CombatMovementEffectConfig.Create(new[]
                {
                    new AddCombatantStatusEffect(targetSelector, statusFactory)
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

        ((TestCombatantStatus)statuses[0]).Value.Should().Be(10);
    }

    private sealed class TestCombatantStatus : CombatantStatusBase
    {
        private readonly Func<ICombatant, int> _valueDelegate;
        private IStatModifier? _statModifier;

        public TestCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
            ICombatantStatusSource source, Func<ICombatant, int> valueDelegate) :
            base(sid, lifetime, source)
        {
            _valueDelegate = valueDelegate;
        }

        public int Value => _statModifier.Value;
        
        public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
        {
            base.Impose(combatant, context);

            _statModifier = new StatModifier(_valueDelegate(((CombatMovementCombatantStatusSource)Source).Actor),
                new NullStatModifierSource());
        }
    }
}