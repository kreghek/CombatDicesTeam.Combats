namespace Core.Combats;

public sealed class CombatantInterruptedEventArgs : CombatantEventArgsBase
{
    public CombatantInterruptedEventArgs(ICombatant combatant) : base(combatant)
    {
    }
}