using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class ToEndOfCurrentRoundEffectLifetimeFactory : ICombatantStatusLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new ToEndOfCurrentRoundEffectLifetime();
    }
}