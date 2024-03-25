using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

[PublicAPI]
public class TargetCombatantsBoundCombatantStatusLifetime : ICombatantStatusLifetime
{
    private readonly IList<ICombatant> _openBoundCombatants;

    public TargetCombatantsBoundCombatantStatusLifetime(params ICombatant[] boundCombatants)
    {
        _openBoundCombatants = new List<ICombatant>(boundCombatants);
    }

    private void Combat_CombatantHasBeenDefeated(object? sender, CombatantDefeatedEventArgs e)
    {
        if (!_openBoundCombatants.Contains(e.Combatant))
        {
            return;
        }

        _openBoundCombatants.Remove(e.Combatant);

        // Then all bound combatant is defeated
        if (!_openBoundCombatants.Any())
        {
            IsExpired = true;
        }
    }

    public void HandleDispelling(ICombatantStatus owner, ICombatantStatusLifetimeDispelContext context)
    {
        context.Combat.CombatantHasBeenDefeated -= Combat_CombatantHasBeenDefeated;
    }

    public void HandleImposed(ICombatantStatus owner, ICombatantStatusLifetimeImposeContext context)
    {
        context.Combat.CombatantHasBeenDefeated += Combat_CombatantHasBeenDefeated;
    }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        // Do nothing.
        // Changing of the status is in the event handlers.
    }

    public bool IsExpired { get; private set; }
}