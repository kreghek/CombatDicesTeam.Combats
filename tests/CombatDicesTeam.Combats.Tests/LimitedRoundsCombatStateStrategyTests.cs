namespace CombatDicesTeam.Combats.Tests;

[TestFixture]
public sealed class LimitedRoundsCombatStateStrategyTests
{
    [Test]
    public void CalculateCurrentState_RoundAmountMoreThatLimit_ReturnsDraw()
    {
        // ARRANGE

        var strategyContext = Mock.Of<ICombatStateStrategyContext>(x => x.CurrentRound == 11);

        var strategy = new LimitedRoundsCombatStateStrategy(Mock.Of<ICombatStateStrategy>(), 10);

        // ACT

        var factState = strategy.CalculateCurrentState(strategyContext);

        // ARRANGE

        factState.Should().BeSameAs(CommonCombatStates.Draw);
    }

    [Test]
    public void CalculateCurrentState_FirstRound_ReturnsBaseStrategyResult()
    {
        // ARRANGE

        var strategyContext = Mock.Of<ICombatStateStrategyContext>(x => x.CurrentRound == 1);

        var baseCombatState = Mock.Of<ICombatState>();

        var strategy = new LimitedRoundsCombatStateStrategy(Mock.Of<ICombatStateStrategy>(strategy =>
            strategy.CalculateCurrentState(It.IsAny<ICombatStateStrategyContext>()) == baseCombatState), 10);

        // ACT

        var factState = strategy.CalculateCurrentState(strategyContext);

        // ARRANGE

        factState.Should().BeSameAs(baseCombatState);
    }
}