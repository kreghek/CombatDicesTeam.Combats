namespace Core.Combats;

public sealed class CombatantShiftShapedEventArgs : CombatantEventArgsBase
{
    public CombatantShiftShapedEventArgs(ICombatant combatant) : base(combatant)
    {
    }
}