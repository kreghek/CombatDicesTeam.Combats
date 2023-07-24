namespace CombatDicesTeam.Combats;

public sealed class CombatantEndsTurnEventArgs : CombatantEventArgsBase
{
    public CombatantEndsTurnEventArgs(ICombatant combatant) : base(combatant)
    {
    }
}