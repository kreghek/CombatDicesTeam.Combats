using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

using FluentAssertions;

namespace CombatDicesTeam.Combats.Tests
{
    [TestFixture]
    public sealed class DamageEffectInstanceTests
    {
        [Test]
        public void AddModifier_ShouldAddModifierToDamageMinAndMax()
        {
            // ARRANGE

            var damageEffectConfig = new DamageEffectConfig(Mock.Of<ICombatantStatType>(), Mock.Of<ICombatantStatType>(), Mock.Of<ICombatantStatType>());

            var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal, new GenericRange<int>(1, 1), damageEffectConfig);
            var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);

            var modifierMock = new Mock<IStatModifier>();

            // ACT

            damageEffectInstance.AddModifier(modifierMock.Object);

            // ASSERT

            damageEffectInstance.Damage.Min.Modifiers.Should().Contain(modifierMock.Object);
            damageEffectInstance.Damage.Max.Modifiers.Should().Contain(modifierMock.Object);
        }

        [Test]
        public void Influence_HasProtectionStat_ShouldDamageTargetBasedOnDamageRange()
        {
            // Arrange

            var mainStatType = Mock.Of<ICombatantStatType>();
            var protectionStatType = Mock.Of<ICombatantStatType>();
            var absorptionStatType = Mock.Of<ICombatantStatType>();

            var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
            var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal, new GenericRange<int>(10, 10), damageEffectConfig);
            var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
            var targetMock = new Mock<ICombatant>();
            targetMock.Setup(t => t.Stats)
                .Returns(new[]
                {
                    Mock.Of<IUnitStat>( s => s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                    Mock.Of<IUnitStat>( s => s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0)),
                    Mock.Of<IUnitStat>( s => s.Type == absorptionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0))
                });

            var statusCombatContextMock = new Mock<IStatusCombatContext>();

            var dice = new Mock<IDice>();

            dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);

            statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

            // Act

            damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

            // Assert

            statusCombatContextMock.Verify(c => c.DamageCombatantStat(targetMock.Object, damageEffectConfig.ProtectionStatType, 10), Times.Once);
        }

        [Test]
        public void Influence_HasNoProtectionStat_ShouldDamageTargetBasedOnDamageRange()
        {
            // Arrange

            var mainStatType = Mock.Of<ICombatantStatType>();
            var protectionStatType = Mock.Of<ICombatantStatType>();
            var absorptionStatType = Mock.Of<ICombatantStatType>();

            var damageEffectConfig = new DamageEffectConfig(mainStatType, protectionStatType, absorptionStatType);
            var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal, new GenericRange<int>(10, 10), damageEffectConfig);
            var damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);
            var targetMock = new Mock<ICombatant>();
            targetMock.Setup(t => t.Stats)
                .Returns(new[] 
                { 
                    Mock.Of<IUnitStat>( s => s.Type == mainStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 10)),
                    Mock.Of<IUnitStat>( s => s.Type == protectionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0)),
                    Mock.Of<IUnitStat>( s => s.Type == absorptionStatType && s.Value == Mock.Of<IStatValue>(v => v.Current == 0))
                });

            var statusCombatContextMock = new Mock<IStatusCombatContext>();
            statusCombatContextMock.Setup(c => c.DamageCombatantStat(targetMock.Object, damageEffectConfig.ProtectionStatType, It.IsAny<int>())).Returns(10);

            var dice = new Mock<IDice>();

            dice.Setup(d => d.Roll(It.IsAny<int>())).Returns(10);
            
            statusCombatContextMock.Setup(c => c.Dice).Returns(dice.Object);

            // Act

            damageEffectInstance.Influence(targetMock.Object, statusCombatContextMock.Object);

            // Assert

            statusCombatContextMock.Verify(c => c.DamageCombatantStat(targetMock.Object, damageEffectConfig.MainStatType, 10), Times.Once);
        }
    }
}
