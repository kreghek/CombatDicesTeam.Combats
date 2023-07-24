namespace CombatDicesTeam.Combats;

public delegate void CombatantHasMovedCallback(FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
    FieldCoords destinationCoords, CombatFieldSide destinationFieldSide);