namespace Core.Combats;

public interface ICombatMovementContainer
{
    ICombatMovementContainerType Type { get; }
    IReadOnlyList<CombatMovementInstance?> GetItems();

    void SetMove(CombatMovementInstance? combatMovement, int index);
    void RemoveAt(int index);
    void AppendMove(CombatMovementInstance? combatMovement);
}