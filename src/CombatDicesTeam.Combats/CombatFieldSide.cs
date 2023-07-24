namespace CombatDicesTeam.Combats;

public sealed class CombatFieldSide
{
    public const int COLUMN_COUNT = 2;
    public const int LINE_COUNT = 3;
    private readonly Matrix<FormationSlot> _matrix;

    public CombatFieldSide()
    {
        _matrix = new Matrix<FormationSlot>(COLUMN_COUNT, LINE_COUNT);

        for (var columnIndex = 0; columnIndex < COLUMN_COUNT; columnIndex++)
        for (var lineIndex = 0; lineIndex < LINE_COUNT; lineIndex++)
        {
            _matrix[columnIndex, lineIndex] = new FormationSlot(columnIndex, lineIndex);
        }
    }

    public int ColumnCount => _matrix.Width;

    public FormationSlot this[FieldCoords coords]
    {
        get => _matrix[coords.ColumentIndex, coords.LineIndex];
        set => _matrix[coords.ColumentIndex, coords.LineIndex] = value;
    }

    public int LineCount => _matrix.Height;
}