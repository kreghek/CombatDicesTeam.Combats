namespace CombatDicesTeam.Combats;

public record CombatantEffectSid(string Value) : ICombatantStatusSid, IComparable
{
    public override string ToString()
    {
        return Value;
    }

    public int CompareTo(object? obj)
    {
        return Value.CompareTo(((CombatantEffectSid?)obj)?.Value);
    }
}