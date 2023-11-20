namespace CombatDicesTeam.Combats;

public class CombatFinishedEventArgs : EventArgs
{
    public CombatFinishedEventArgs(ICombatState combatState)
    {
        Result = combatState;
    }

    public ICombatState Result { get; }
}