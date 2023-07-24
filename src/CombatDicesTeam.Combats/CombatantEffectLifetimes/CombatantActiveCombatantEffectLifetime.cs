namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public sealed class CombatantActiveCombatantEffectLifetime : ICombatantStatusLifetime
{
    private void CombatCore_CombatantHasBeenDefeated(object? sender, CombatantDefeatedEventArgs e)
    {
        IsExpired = true;
    }

    /// <inheritdoc />
    public bool IsExpired { get; private set; }

    public void HandleDispelling(ICombatantStatus combatantEffect, ICombatantStatusLifetimeDispelContext context)
    {
        context.Combat.CombatantHasBeenDefeated -= CombatCore_CombatantHasBeenDefeated;
    }

    public void HandleImposed(ICombatantStatus combatantEffect, ICombatantStatusLifetimeImposeContext context)
    {
        context.Combat.CombatantHasBeenDefeated += CombatCore_CombatantHasBeenDefeated;
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
    }
}