using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed record CombatantStatusLifetimeUpdateContext
    (ICombatant Combatant, CombatEngineBase Combat) : ICombatantStatusLifetimeUpdateContext
{
    public void CompleteTurn()
    {
        Combat.Interrupt();
    }
}