namespace CombatDicesTeam.Combats;

public class FormationSlot
{
    public FormationSlot(int columnIndex, int lineIndex)
    {
        ColumnIndex = columnIndex;
        LineIndex = lineIndex;
    }

    public int ColumnIndex { get; }

    public ICombatant? Combatant { get; set; }
    public int LineIndex { get; }
}