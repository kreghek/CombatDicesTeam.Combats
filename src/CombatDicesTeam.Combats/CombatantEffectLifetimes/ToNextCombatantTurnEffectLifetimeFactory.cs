namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public sealed class ToNextCombatantTurnEffectLifetimeFactory : ICombatantStatusLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new ToNextCombatantTurnEffectLifetime();
    }
}