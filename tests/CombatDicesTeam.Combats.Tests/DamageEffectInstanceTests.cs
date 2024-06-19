using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Tests;

[TestFixture]
public sealed class DamageEffectInstanceTests
{
    [Test]
    public void AddModifier_ShouldAddModifierToDamageMinAndMax()
    {
        // ARRANGE

        var damageEffectConfig = new DamageEffectConfig(Mock.Of<ICombatantStatType>(),
            Mock.Of<ICombatantStatType>(), Mock.Of<ICombatantStatType>());

        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(1, 1), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);

        var modifierMock = new Mock<IStatModifier>();

        // ACT

        damageEffectInstance.AddModifier(modifierMock.Object);

        // ASSERT

        damageEffectInstance.Damage.Min.Modifiers.Should().Contain(modifierMock.Object);
        damageEffectInstance.Damage.Max.Modifiers.Should().Contain(modifierMock.Object);
    }

    [Test]
    public void RemoveModifier_ShouldRemoveModifierToDamageMinAndMax()
    {
        // ARRANGE

        var damageEffectConfig = new DamageEffectConfig(Mock.Of<ICombatantStatType>(),
            Mock.Of<ICombatantStatType>(), Mock.Of<ICombatantStatType>());

        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(1, 1), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);

        var modifierMock = new Mock<IStatModifier>();

        damageEffectInstance.AddModifier(modifierMock.Object);

        // ACT

        damageEffectInstance.RemoveModifier(modifierMock.Object);

        // ASSERT

        damageEffectInstance.Damage.Min.Modifiers.Should().NotContain(modifierMock.Object);
        damageEffectInstance.Damage.Max.Modifiers.Should().NotContain(modifierMock.Object);
    }

    [Test]
    public void Influence_HasProtectionStat_ShouldDamageTargetBasedOnDamageRange()
    {
        // Arrange

        var mainStatType = Mock.Of<ICombatantStatType>();
        var protectionStatType = Mock.Of<ICombatantStatType>();
        var absorptionStatType = Mock.Of<ICombatantStatType>();

        var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(10, 10), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
        var targetMock = new Mock<ICombatant>();
        targetMock.Setup(t => t.Stats)
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(s =>
                    s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == absorptionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0))
            });

        var statusCombatContextMock = new Mock<ICombatMovementContext>();

        var dice = new Mock<IDice>();

        dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

        statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

        // Act

        damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

        // Assert

        statusCombatContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, It.IsAny<IStatChangingSource>(),
                damageEffectConfig.ProtectionStatType, 10), Times.Once);
    }

    [Test]
    public void Influence_HasProtectionStatEqualsDamage_ShouldNotMakeZeroDamage()
    {
        // Arrange

        var mainStatType = Mock.Of<ICombatantStatType>();
        var protectionStatType = Mock.Of<ICombatantStatType>();
        var absorptionStatType = Mock.Of<ICombatantStatType>();

        var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(10, 10), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
        var targetMock = new Mock<ICombatant>();
        targetMock.Setup(t => t.Stats)
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(s =>
                    s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == absorptionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0))
            });

        var statusCombatContextMock = new Mock<ICombatMovementContext>();

        var dice = new Mock<IDice>();

        dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

        statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

        // Act

        damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

        // Assert

        // Because protection consumes all of the damage.
        statusCombatContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, Mock.Of<IStatChangingSource>(),
                damageEffectConfig.MainStatType,
                It.IsAny<StatDamage>()),
            Times.Never);
    }

    [Test]
    public void Influence_HasNoProtectionStat_ShouldDamageTargetBasedOnDamageRange()
    {
        // Arrange

        var mainStatType = Mock.Of<ICombatantStatType>();
        var protectionStatType = Mock.Of<ICombatantStatType>();
        var absorptionStatType = Mock.Of<ICombatantStatType>();

        var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(10, 10), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
        var targetMock = new Mock<ICombatant>();
        targetMock.Setup(t => t.Stats)
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(s =>
                    s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == absorptionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0))
            });

        var statusCombatContextMock = new Mock<ICombatMovementContext>();
        statusCombatContextMock.Setup(c =>
                c.DamageCombatantStat(targetMock.Object, It.IsAny<IStatChangingSource>(),
                    damageEffectConfig.ProtectionStatType,
                    It.IsAny<StatDamage>()))
            .Returns(10);

        var dice = new Mock<IDice>();

        dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

        statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

        // Act

        damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

        // Assert

        statusCombatContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, It.IsAny<IStatChangingSource>(),
                damageEffectConfig.MainStatType,
                It.Is<StatDamage>(x => x.Amount == 10)), Times.Once);
    }

    [Test]
    public void Influence_HasAbsorptionStat_ShouldDamageTargetBasedOnDamageRange()
    {
        // Arrange

        var mainStatType = Mock.Of<ICombatantStatType>();
        var protectionStatType = Mock.Of<ICombatantStatType>();
        var absorptionStatType = Mock.Of<ICombatantStatType>();

        var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(10, 10), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
        var targetMock = new Mock<ICombatant>();
        targetMock.Setup(t => t.Stats)
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(s =>
                    s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == absorptionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 1))
            });

        var combatMovementContextMock = new Mock<ICombatMovementContext>();
        combatMovementContextMock.Setup(c =>
                c.DamageCombatantStat(targetMock.Object, Mock.Of<IStatChangingSource>(),
                    damageEffectConfig.ProtectionStatType,
                    It.IsAny<StatDamage>()))
            .Returns(10);

        var dice = new Mock<IDice>();

        dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

        combatMovementContextMock.Setup(c => c.Dice).Returns(dice.Object);

        // Act

        damageEffectInstance.Influence(targetMock.Object, combatMovementContextMock.Object);

        // Assert

        combatMovementContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, It.IsAny<IStatChangingSource>(),
                damageEffectConfig.ProtectionStatType,
                It.Is<StatDamage>(x => x.Amount == 9)), Times.Once);
    }

    [Test]
    public void Influence_HasCurrentAbsorptionStat_ShouldReduceDamageOnCurrentInsteadMaximumUnmodifiedAbsorption()
    {
        // Arrange

        var mainStatType = Mock.Of<ICombatantStatType>();
        var protectionStatType = Mock.Of<ICombatantStatType>();
        var absorptionStatType = Mock.Of<ICombatantStatType>();

        var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(10, 10), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
        var targetMock = new Mock<ICombatant>();
        targetMock.Setup(t => t.Stats)
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(s =>
                    s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == absorptionStatType &&
                    s.Value == Mock.Of<IStatValue>(v => v.Current == 1 && v.ActualMax == 2))
            });

        var statusCombatContextMock = new Mock<ICombatMovementContext>();

        var dice = new Mock<IDice>();

        dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

        statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

        // Act

        damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

        // Assert

        statusCombatContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, It.IsAny<IStatChangingSource>(),
                damageEffectConfig.ProtectionStatType,
                It.Is<StatDamage>(x => x.Amount == 9)), Times.Once);
        statusCombatContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, It.IsAny<IStatChangingSource>(),
                damageEffectConfig.ProtectionStatType,
                It.Is<StatDamage>(x => x.Amount == 8)), Times.Never);
    }

    [Test]
    public void
        Influence_ProtectionOnlyDamageAboveProtectionCurrentValue_ShouldDamageMainStatWithZeroAmountAndNonZeroSourceAmount()
    {
        // Arrange

        var mainStatType = Mock.Of<ICombatantStatType>();
        var protectionStatType = Mock.Of<ICombatantStatType>();
        var absorptionStatType = Mock.Of<ICombatantStatType>();

        var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.ProtectionOnly,
            new GenericRange<int>(10, 10), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
        var targetMock = new Mock<ICombatant>();
        targetMock.Setup(t => t.Stats)
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(s =>
                    s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 1)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == absorptionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0))
            });

        var statusCombatContextMock = new Mock<ICombatMovementContext>();
        statusCombatContextMock.Setup(c =>
                c.DamageCombatantStat(targetMock.Object, Mock.Of<IStatChangingSource>(),
                    damageEffectConfig.ProtectionStatType,
                    It.IsAny<StatDamage>()))
            .Returns(10);

        var dice = new Mock<IDice>();

        dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

        statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

        // Act

        damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

        // Assert

        statusCombatContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, Mock.Of<IStatChangingSource>(),
                damageEffectConfig.MainStatType,
                It.Is<StatDamage>(x => x.Amount == 0 && x.SourceAmount != 0)),
            Times.Never);
    }

    [Test]
    public void Influence_HasNoAbsorbtionStat_AbsorbtionIgnored()
    {
        // Arrange

        var mainStatType = Mock.Of<ICombatantStatType>();
        var protectionStatType = Mock.Of<ICombatantStatType>();
        var absorptionStatType = Mock.Of<ICombatantStatType>();

        var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
        var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal,
            new GenericRange<int>(10, 10), damageEffectConfig);
        var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
        var targetMock = new Mock<ICombatant>();
        targetMock.Setup(t => t.Stats)
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(s =>
                    s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                Mock.Of<ICombatantStat>(s =>
                    s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10))
            });

        var statusCombatContextMock = new Mock<ICombatMovementContext>();

        var dice = new Mock<IDice>();

        dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

        statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

        // Act

        damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

        // Assert

        statusCombatContextMock.Verify(
            c => c.DamageCombatantStat(targetMock.Object, It.IsAny<IStatChangingSource>(),
                damageEffectConfig.ProtectionStatType,
                It.IsAny<StatDamage>()),
            Times.Once);
    }
}