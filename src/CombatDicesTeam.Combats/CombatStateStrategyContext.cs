namespace CombatDicesTeam.Combats;

public sealed class CombatStateStrategyContext: ICombatStateStrategyContext
{
    public CombatStateStrategyContext(IReadOnlyCollection<ICombatant> combatants)
    {
        Combatants = combatants;
    }

    public IReadOnlyCollection<ICombatant> Combatants { get; }
}