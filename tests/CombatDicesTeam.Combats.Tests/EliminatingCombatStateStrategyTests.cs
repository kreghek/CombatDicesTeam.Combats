using FluentAssertions;

namespace CombatDicesTeam.Combats.Tests;

[TestFixture]
public class EliminatingCombatStateStrategyTests
{
    [Test]
    public void CalculateCurrentState_OnlyPlayerCombatants_ReturnsVictory()
    {
        // ARRANGE

        var strategyContext = Mock.Of<ICombatStateStrategyContext>(x => x.Combatants == new[]
        {
            Mock.Of<ICombatant>(c => c.IsPlayerControlled == true)
        });

        var strategy = new EliminatingCombatStateStrategy();

        // ACT

        var factState = strategy.CalculateCurrentState(strategyContext);

        // ASSERT

        factState.Should().BeSameAs(CommonCombatStates.Victory);
    }

    [Test]
    public void CalculateCurrentState_OnlyPlayerCombatantsAlive_ReturnsVictory()
    {
        // ARRANGE

        var strategyContext = Mock.Of<ICombatStateStrategyContext>(x => x.Combatants == new[]
        {
            Mock.Of<ICombatant>(c => c.IsPlayerControlled == true),
            Mock.Of<ICombatant>(c => c.IsPlayerControlled == false && c.IsDead == true)
        });

        var strategy = new EliminatingCombatStateStrategy();

        // ACT

        var factState = strategy.CalculateCurrentState(strategyContext);

        // ASSERT

        factState.Should().BeSameAs(CommonCombatStates.Victory);
    }

    [Test]
    public void CalculateCurrentState_OnlyCpuCombatants_ReturnsFailure()
    {
        // ARRANGE

        var strategyContext = Mock.Of<ICombatStateStrategyContext>(x => x.Combatants == new[]
        {
            Mock.Of<ICombatant>(c => c.IsPlayerControlled == false)
        });

        var strategy = new EliminatingCombatStateStrategy();

        // ACT

        var factState = strategy.CalculateCurrentState(strategyContext);

        // ASSERT

        factState.Should().BeSameAs(CommonCombatStates.Failure);
    }

    [Test]
    public void CalculateCurrentState_PlayerAndCpuCombatantsAlive_ReturnsInProgress()
    {
        // ARRANGE

        var strategyContext = Mock.Of<ICombatStateStrategyContext>(x => x.Combatants == new[]
        {
            Mock.Of<ICombatant>(c => c.IsPlayerControlled == true),
            Mock.Of<ICombatant>(c => c.IsPlayerControlled == false)
        });

        var strategy = new EliminatingCombatStateStrategy();

        // ACT

        var factState = strategy.CalculateCurrentState(strategyContext);

        // ASSERT

        factState.Should().BeSameAs(CommonCombatStates.InProgress);
    }
}