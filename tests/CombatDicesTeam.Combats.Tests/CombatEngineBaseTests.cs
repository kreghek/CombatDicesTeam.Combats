using CombatDicesTeam.Dices;

using FluentAssertions;

namespace CombatDicesTeam.Combats.Tests;

[TestFixture]
public class CombatEngineBaseTests
{
    [Test]
    public void CompleteTurn_Draw_CombatFinishedEventRaised()
    {
        // ARRANGE

        var roundQueueResolver = Mock.Of<IRoundQueueResolver>(x => x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()) == new[] { Mock.Of<ICombatant>() });
        var combatState = Mock.Of<ICombatState>(cs => cs.IsFinalState == true);
        var stateStrategy = Mock.Of<ICombatStateStrategy>(x => x.CalculateCurrentState(It.IsAny<ICombatStateStrategyContext>()) == combatState);
        var combat = new TestableCombatEngine(Mock.Of<IDice>(), roundQueueResolver, stateStrategy);

        var heroes = new[] { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>() } };

        var monsters = new[] { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>() } };

        combat.Initialize(heroes, monsters);

        using var monitor = combat.Monitor();

        // ACT

        combat.CompleteTurn();

        // ASSERT

        monitor.Should().Raise(nameof(combat.CombatFinished)).WithArgs<CombatFinishedEventArgs>(e => e.Result.IsFinalState == true);
    }
    
    // TODO Move use case tests to separate project
    [Test]
    public void CompleteTurn_MultipleTurns_CombatFinishedEventRaised()
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

        var heroes = new[] { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>(c => c.IsPlayerControlled == true) } };

        var monsters = new[] { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>(c => c.IsPlayerControlled == false) } };

        combat.Initialize(heroes, monsters);

        using var monitor = combat.Monitor();

        // ACT

        for (var roundIndex = 0; roundIndex < MAX_ROUNDS; roundIndex++)
        {
            for (var combatantIndex = 0; combatantIndex < combat.CurrentCombatants.Count; combatantIndex++)
            {
                combat.CompleteTurn();       
            }
        }

        // ASSERT

        monitor.Should().Raise(nameof(combat.CombatFinished)).WithArgs<CombatFinishedEventArgs>(e => e.Result == CommonCombatStates.Draw);
    }
}
