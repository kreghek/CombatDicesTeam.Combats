using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.Tests;

[TestFixture]
public class CombatEngineBaseTests
{
    [Test]
    public void CompleteTurn_Draw_CombatFinishedEventRaised()
    {
        // ARRANGE

        var roundQueueResolver = Mock.Of<IRoundQueueResolver>(x =>
            x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()) == new[] { Mock.Of<ICombatant>() });
        var combatState = Mock.Of<ICombatState>(cs => cs.IsFinalState == true);
        var stateStrategy = Mock.Of<ICombatStateStrategy>(x =>
            x.CalculateCurrentState(It.IsAny<ICombatStateStrategyContext>()) == combatState);
        var combat = new TestableCombatEngine(Mock.Of<IDice>(), roundQueueResolver, stateStrategy);

        var heroes = new[] { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>() } };

        var monsters = new[] { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>() } };

        combat.Initialize(heroes, monsters);

        using var monitor = combat.Monitor();

        // ACT

        combat.CompleteTurn();

        // ASSERT

        monitor.Should().Raise(nameof(combat.CombatFinished))
            .WithArgs<CombatFinishedEventArgs>(e => e.Result.IsFinalState == true);
    }

    [Test]
    public void HandleCombatantDamagedToStat()
    {
        const int DAMAGE = 10;
        const int STAT = 1;

        var roundQueueResolver = Mock.Of<IRoundQueueResolver>(x =>
            x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()) == new[] { Mock.Of<ICombatant>() });
        var combatState = Mock.Of<ICombatState>(cs => cs.IsFinalState == true);
        var stateStrategy = Mock.Of<ICombatStateStrategy>(x =>
            x.CalculateCurrentState(It.IsAny<ICombatStateStrategyContext>()) == combatState);
        var sut = new TestableCombatEngine(Mock.Of<IDice>(), roundQueueResolver, stateStrategy);

        using var monitor = sut.Monitor();

        var statType = Mock.Of<ICombatantStatType>();
        var stat = new CombatantStat(statType, new StatValue(STAT));
        var combatant = Mock.Of<ICombatant>(x => x.Stats == new[] { stat });

        // ACT

        var _ = sut.TestHandleCombatantDamagedToStat(combatant, Mock.Of<IStatChangingSource>(), statType, DAMAGE);

        // ASSERT

        monitor.Should().Raise(nameof(sut.CombatantStatChanged))
            .WithArgs<CombatantDamagedEventArgs>(x => x.Damage.Amount == STAT);
    }

    [Test]
    public void HandleCombatantDamagedToStat2()
    {
        const int DAMAGE = 1;
        const int STAT = 10;

        var roundQueueResolver = Mock.Of<IRoundQueueResolver>(x =>
            x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()) == new[] { Mock.Of<ICombatant>() });
        var combatState = Mock.Of<ICombatState>(cs => cs.IsFinalState == true);
        var stateStrategy = Mock.Of<ICombatStateStrategy>(x =>
            x.CalculateCurrentState(It.IsAny<ICombatStateStrategyContext>()) == combatState);
        var sut = new TestableCombatEngine(Mock.Of<IDice>(), roundQueueResolver, stateStrategy);

        using var monitor = sut.Monitor();

        var statType = Mock.Of<ICombatantStatType>();
        var stat = new CombatantStat(statType, new StatValue(STAT));
        var combatant = Mock.Of<ICombatant>(x => x.Stats == new[] { stat });

        // ACT

        var _ = sut.TestHandleCombatantDamagedToStat(combatant, Mock.Of<IStatChangingSource>(), statType, DAMAGE);

        // ASSERT

        monitor.Should().Raise(nameof(sut.CombatantStatChanged))
            .WithArgs<CombatantDamagedEventArgs>(x => x.Damage.Amount == DAMAGE);
    }
}
