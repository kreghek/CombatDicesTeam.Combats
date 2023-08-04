namespace CombatDicesTeam.Combats;

public interface IRoundQueueResolver
{
    IReadOnlyList<ICombatant> GetCurrentRoundQueue(IReadOnlyCollection<ICombatant> combatants);
}