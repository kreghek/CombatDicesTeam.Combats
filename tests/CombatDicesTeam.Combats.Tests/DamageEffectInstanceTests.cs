using CombatDicesTeam.Combats.Effects;

namespace CombatDicesTeam.Combats.Tests
{
    [TestFixture]
    public sealed class DamageEffectInstanceTests
    {
        private Mock<IStatValue> _minStatValueMock;
        private Mock<IStatValue> _maxStatValueMock;

        private DamageEffectInstance _damageEffectInstance;

        [SetUp]
        public void SetUp() 
        {
            var damageEffectConfig = new DamageEffectConfig(Mock.Of<ICombatantStatType>(), Mock.Of<ICombatantStatType>(), Mock.Of<ICombatantStatType>());
            _minStatValueMock = new Mock<IStatValue>();
            _maxStatValueMock = new Mock<IStatValue>();

            var damageEffect = new DamageEffect(Mock.Of<ITargetSelector>(), DamageType.Normal, new GenericRanges.GenericRange<int>(1, 1), damageEffectConfig);
            _damageEffectInstance = new DamageEffectInstance(damageEffect, damageEffectConfig);

            
        }

        [Test]
        public void AddModifier_ShouldAddModifierToDamageMinAndMax()
        {
            // ARRANGE

            var modifierMock = new Mock<IUnitStatModifier>();

            // ACT

            _damageEffectInstance.AddModifier(modifierMock.Object);

            // ASSERT

            _minStatValueMock.Verify(x => x.AddModifier(modifierMock.Object), Times.Once);
            _maxStatValueMock.Verify(x => x.AddModifier(modifierMock.Object), Times.Once);
        }
    }
}
