namespace Core.Combats.CombatantEffectLifetimes;

public sealed class ToEndOfCurrentRoundEffectLifetime : ICombatantStatusLifetime
{
    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        if (updateType == CombatantStatusUpdateType.EndRound)
        {
            IsExpired = true;
        }
    }

    public void HandleImposed(ICombatantStatus combatantEffect, ICombatantStatusLifetimeImposeContext context)
    {
    }

    public void HandleDispelling(ICombatantStatus combatantEffect, ICombatantStatusLifetimeDispelContext context)
    {
    }

    public bool IsExpired { get; private set; }
}

public sealed class ToEndOfCurrentRoundEffectLifetimeFactory : ICombatantStatusLifetimeFactory
{
    public ICombatantStatusLifetime Create()
    {
        return new ToEndOfCurrentRoundEffectLifetime();
    }
}