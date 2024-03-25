using CombatDicesTeam.Combats.CombatantEffectLifetimes;

namespace CombatDicesTeam.Combats.Tests.CombatantEffectLifetimes;

public class TargetCombatantsBoundCombatantStatusLifetimeTests
{
    [Test]
    public void IsExpired_sets_true_when_every_bound_combatants_are_defeated()
    {
        // ASSERT

        var boundCombatant = Mock.Of<ICombatant>();
        var targetCombatant = Mock.Of<ICombatant>();

        var status = Mock.Of<ICombatantStatus>();

        var combatMock = new Mock<CombatEngineBase>();
        //combatMock.Setup(x => x.CurrentCombatants).Returns(new[] { boundCombatant, targetCombatant });
        var combat = combatMock.Object;

        var sut = new TargetCombatantsBoundCombatantStatusLifetime(boundCombatant);
        
        // ACT

        sut.HandleImposed(status,
            Mock.Of<ICombatantStatusLifetimeImposeContext>(x =>
                x.TargetCombatant == targetCombatant && x.Combat == combat));

        combatMock.Raise(x => x.CombatantHasBeenDefeated += null, new CombatantDefeatedEventArgs(boundCombatant));
        
        // ASSERT

        sut.IsExpired.Should().BeTrue();
    }
}