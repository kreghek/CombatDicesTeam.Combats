using CombatDicesTeam.Combats;

namespace SampleCombat;

public sealed class CombatMovementContainer : ICombatMovementContainer
{
    private readonly IList<CombatMovementInstance?> _items;

    public CombatMovementContainer(ICombatMovementContainerType type)
    {
        Type = type;
        _items = new List<CombatMovementInstance?>();
    }

    public ICombatMovementContainerType Type { get; }

    public IReadOnlyList<CombatMovementInstance?> GetItems()
    {
        return _items.ToArray();
    }

    public void AppendMove(CombatMovementInstance? combatMovement)
    {
        _items.Add(combatMovement);
    }

    public void SetMove(CombatMovementInstance? combatMovement, int index)
    {
        _items[index] = combatMovement;
    }

    public void RemoveAt(int index)
    {
        _items.RemoveAt(index);
    }
}