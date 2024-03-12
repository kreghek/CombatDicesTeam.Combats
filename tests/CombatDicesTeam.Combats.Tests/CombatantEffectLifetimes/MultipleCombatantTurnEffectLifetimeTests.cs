using NUnit.Framework;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes.Tests;

[TestFixture()]
public class MultipleCombatantTurnEffectLifetimeTests
{
    [Test()]
    public void UpdateTest()
    {
        // ARRANGE

        var sut = new MultipleCombatantTurnEffectLifetime(1);

        // ACT

        sut.Update(CombatantStatusUpdateType.EndCombatantTurn, Mock.Of<ICombatantStatusLifetimeUpdateContext>());

        // ASSERT

        sut.IsExpired.Should().BeFalse();
    }

    [Test()]
    public void UpdateTest2()
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

    [Test()]
    public void UpdateTest3()
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