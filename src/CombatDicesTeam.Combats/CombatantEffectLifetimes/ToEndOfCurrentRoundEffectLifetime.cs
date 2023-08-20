using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
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
