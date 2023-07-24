namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public sealed class CombatantActiveCombatantEffectLifetimeFactory : ICombatantStatusLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new CombatantActiveCombatantEffectLifetime();
    }
}