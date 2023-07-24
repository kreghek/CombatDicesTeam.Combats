namespace Core.Combats;

public sealed class CombatantHasChangedPositionEventArgs : CombatantEventArgsBase
{
    public CombatantHasChangedPositionEventArgs(ICombatant combatant, CombatFieldSide fieldSide,
        FieldCoords newFieldCoords) :
        base(combatant)
    {
        FieldSide = fieldSide;
        NewFieldCoords = newFieldCoords;
    }

    public CombatFieldSide FieldSide { get; }
    public FieldCoords NewFieldCoords { get; }
}