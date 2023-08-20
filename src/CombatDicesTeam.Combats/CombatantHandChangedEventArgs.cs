using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed class CombatantHandChangedEventArgs : CombatantEventArgsBase
{
    public CombatantHandChangedEventArgs(ICombatant combatant, CombatMovementInstance move, int handSlotIndex) :
        base(combatant)
    {
        Move = move;
        HandSlotIndex = handSlotIndex;
    }

    [PublicAPI]
    public int HandSlotIndex { get; }

    [PublicAPI]
    public CombatMovementInstance Move { get; }
}