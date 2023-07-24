namespace Core.Combats;

public sealed class CombatMovementSequence
{
    public CombatMovementSequence()
    {
        Items = new List<CombatMovement>();
    }

    public IList<CombatMovement> Items { get; }
}