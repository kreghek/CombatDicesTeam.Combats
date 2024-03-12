namespace CombatDicesTeam.Combats.CombatantEffectLifetimes.Tests;

[TestFixture]
public class MultipleCombatantTurnEffectLifetimeTests
{
    [Test]
    public void Update_on_this_combatant_turn_does_not_make_lifetime_expired()
    {
        // ARRANGE

        var sut = new MultipleCombatantTurnEffectLifetime(1);

        // ACT

        sut.Update(CombatantStatusUpdateType.EndCombatantTurn, Mock.Of<ICombatantStatusLifetimeUpdateContext>());

        // ASSERT

        sut.IsExpired.Should().BeFalse();
    }

    [Test]
    public void Update_on_second_combatant_turn_make_lifetime_expired()
    {
        // ARRANGE

        var sut = new MultipleCombatantTurnEffectLifetime(1);

        // ACT

        sut.Update(CombatantStatusUpdateType.EndCombatantTurn, Mock.Of<ICombatantStatusLifetimeUpdateContext>());
        sut.Update(CombatantStatusUpdateType.EndRound, Mock.Of<ICombatantStatusLifetimeUpdateContext>());
        sut.Update(CombatantStatusUpdateType.EndCombatantTurn, Mock.Of<ICombatantStatusLifetimeUpdateContext>());

        // ASSERT

        sut.IsExpired.Should().BeTrue();
    }

    [Test]
    public void Update_on_this_round_does_not_make_lifetime_expired()
    {
        // ARRANGE

        var sut = new MultipleCombatantTurnEffectLifetime(1);

        // ACT

        sut.Update(CombatantStatusUpdateType.EndCombatantTurn, Mock.Of<ICombatantStatusLifetimeUpdateContext>());
        sut.Update(CombatantStatusUpdateType.EndRound, Mock.Of<ICombatantStatusLifetimeUpdateContext>());

        // ASSERT

        sut.IsExpired.Should().BeFalse();
    }
}