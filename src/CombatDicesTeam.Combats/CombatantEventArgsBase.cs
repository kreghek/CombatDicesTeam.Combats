namespace CombatDicesTeam.Combats;

public abstract class CombatantEventArgsBase : EventArgs
{
    protected CombatantEventArgsBase(ICombatant combatant)
    {
        Combatant = combatant;
    }

    public ICombatant Combatant { get; }
}