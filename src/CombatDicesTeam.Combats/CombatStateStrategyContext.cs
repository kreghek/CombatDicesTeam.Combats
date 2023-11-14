namespace CombatDicesTeam.Combats;

public sealed class CombatStateStrategyContext : ICombatStateStrategyContext
{
    public CombatStateStrategyContext(IReadOnlyCollection<ICombatant> combatants, int currentRound)
    {
        Combatants = combatants;
        CurrentRound = currentRound;
    }

    public IReadOnlyCollection<ICombatant> Combatants { get; }
    public int CurrentRound { get; }
}