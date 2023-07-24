namespace CombatDicesTeam.Combats;

public interface ICombatantStatusLifetimeUpdateContext
{
    ICombatant Combatant { get; }
    void CompleteTurn();
}