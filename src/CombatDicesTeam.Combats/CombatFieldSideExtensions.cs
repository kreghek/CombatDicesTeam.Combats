namespace CombatDicesTeam.Combats;

public static class CombatFieldSideExtensions
{
    public static IReadOnlyCollection<ICombatant> GetAllCombatants(this CombatFieldSide side)
    {
        return GetCombatantsIterator(side, _ => true).ToArray();
    }

    public static IReadOnlyCollection<ICombatant> GetColumnCombatants(this CombatFieldSide side, int columnIndex)
    {
        return GetCombatantsIterator(side, combatant =>
        {
            var coords = side.GetCombatantCoords(combatant);

            return coords.ColumentIndex == columnIndex;
        }).ToArray();
    }

    public static FieldCoords GetCombatantCoords(this CombatFieldSide side, ICombatant combatant)
    {
        for (var colIndex = 0; colIndex < side.ColumnCount; colIndex++)
        for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
        {
            var fieldCoords = new FieldCoords(colIndex, lineIndex);
            if (side[fieldCoords].Combatant == combatant)
            {
                return fieldCoords;
            }
        }

        throw new ArgumentException("Not found", nameof(combatant));
    }

    private static IEnumerable<ICombatant> GetCombatantsIterator(CombatFieldSide side, Func<ICombatant, bool> predicate)
    {
        for (var colIndex = 0; colIndex < side.ColumnCount; colIndex++)
        for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
        {
            var combatant = side[new FieldCoords(colIndex, lineIndex)].Combatant;
            if (combatant is not null)
            {
                if (predicate(combatant))
                {
                    yield return combatant;
                }
            }
        }
    }
}