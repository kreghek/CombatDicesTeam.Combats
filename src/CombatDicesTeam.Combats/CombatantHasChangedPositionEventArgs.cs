using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed class CombatantHasChangedPositionEventArgs : CombatantEventArgsBase
{
    public CombatantHasChangedPositionEventArgs(ICombatant combatant, CombatFieldSide fieldSide,
        FieldCoords newFieldCoords) :
        base(combatant)
    {
        FieldSide = fieldSide;
        NewFieldCoords = newFieldCoords;
    }

    [PublicAPI]
    public CombatFieldSide FieldSide { get; }

    [PublicAPI]
    public FieldCoords NewFieldCoords { get; }
}