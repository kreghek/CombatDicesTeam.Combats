namespace CombatDicesTeam.Combats;

public sealed class CombatantHandChangedEventArgs : CombatantEventArgsBase
{
    public CombatantHandChangedEventArgs(ICombatant combatant, CombatMovementInstance move, int handSlotIndex) :
        base(combatant)
    {
        Move = move;
        HandSlotIndex = handSlotIndex;
    }

    public int HandSlotIndex { get; }

    public CombatMovementInstance Move { get; }
}