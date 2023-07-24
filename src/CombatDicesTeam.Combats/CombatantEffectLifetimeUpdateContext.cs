namespace CombatDicesTeam.Combats;

public sealed record CombatantEffectLifetimeUpdateContext
    (ICombatant Combatant, CombatEngineBase Combat) : ICombatantStatusLifetimeUpdateContext
{
    public void CompleteTurn()
    {
        Combat.Interrupt();
    }
}