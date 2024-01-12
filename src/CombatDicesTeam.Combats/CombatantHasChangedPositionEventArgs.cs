using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed class CombatantHasChangedPositionEventArgs : CombatantEventArgsBase
{
    public CombatantHasChangedPositionEventArgs(ICombatant combatant, CombatFieldSide fieldSide,
        FieldCoords newFieldCoords, IPositionChangingReason reason) :
        base(combatant)
    {
        FieldSide = fieldSide;
        NewFieldCoords = newFieldCoords;
        Reason = reason;
    }

    [PublicAPI]
    public CombatFieldSide FieldSide { get; }

    [PublicAPI]
    public FieldCoords NewFieldCoords { get; }

    [PublicAPI]
    public IPositionChangingReason Reason { get; }
}