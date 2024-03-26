using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class OwnerBoundCombatantStatusLifetimeFactory : ICombatantStatusLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new OwnerBoundCombatantEffectLifetime();
    }
}