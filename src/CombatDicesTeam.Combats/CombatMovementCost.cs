namespace Core.Combats;

public sealed record CombatMovementCost(IStatValue Amount)
{
    public CombatMovementCost(int amount) : this(new StatValue(amount))
    {
    }

    public bool HasCost => Amount.Current > 0;
}