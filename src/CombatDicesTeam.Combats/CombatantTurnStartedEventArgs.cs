namespace CombatDicesTeam.Combats;

public sealed class CombatantTurnStartedEventArgs : CombatantEventArgsBase
{
    public CombatantTurnStartedEventArgs(ICombatant combatant) : base(combatant)
    {
    }
}