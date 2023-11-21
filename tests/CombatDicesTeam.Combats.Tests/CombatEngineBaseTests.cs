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
}
