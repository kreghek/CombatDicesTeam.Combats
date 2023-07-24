namespace CombatDicesTeam.Combats;

public class CombatFinishedEventArgs : EventArgs
{
    public CombatFinishedEventArgs(CombatFinishResult result)
    {
        Result = result;
    }

    public CombatFinishResult Result { get; }
}