using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public sealed class OwnerBoundCombatantEffectLifetime : ICombatantStatusLifetime
{
    private ICombatant? _owner;

    private void CombatCore_CombatantHasBeenDefeated(object? sender, CombatantDefeatedEventArgs e)
    {
        if (_owner == sender)
        {
            IsExpired = true;
        }
    }

    /// <inheritdoc />
    public bool IsExpired { get; private set; }

    public void HandleDispelling(ICombatantStatus combatantEffect, ICombatantStatusLifetimeDispelContext context)
    {
        context.Combat.CombatantHasBeenDefeated -= CombatCore_CombatantHasBeenDefeated;
    }

    public void HandleImposed(ICombatantStatus combatantEffect, ICombatantStatusLifetimeImposeContext context)
    {
        _owner = context.TargetCombatant;
        context.Combat.CombatantHasBeenDefeated += CombatCore_CombatantHasBeenDefeated;
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
    }
}