namespace CombatDicesTeam.Combats;

public interface ICombatMovementContainer
{
    ICombatMovementContainerType Type { get; }
    void AppendMove(CombatMovementInstance? combatMovement);
    IReadOnlyList<CombatMovementInstance?> GetItems();
    void RemoveAt(int index);

    void SetMove(CombatMovementInstance? combatMovement, int index);
}