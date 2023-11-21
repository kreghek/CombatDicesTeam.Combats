using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats.UseCaseTests;

[TestFixture]
public class FinishCombatTests
{
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

        var heroes = new[]
            { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>(c => c.IsPlayerControlled == true) } };

        var monsters = new[]
            { new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>(c => c.IsPlayerControlled == false) } };

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

        monitor.Should().Raise(nameof(combat.CombatFinished))
            .WithArgs<CombatFinishedEventArgs>(e => e.Result == CommonCombatStates.Draw);
    }
}