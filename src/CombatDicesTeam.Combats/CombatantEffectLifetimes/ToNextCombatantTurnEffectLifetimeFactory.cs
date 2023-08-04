using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class ToNextCombatantTurnEffectLifetimeFactory : ICombatantStatusLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new ToNextCombatantTurnEffectLifetime();
    }
}