using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public record CombatantStatusSid(string Value) : ICombatantStatusSid, IComparable
{
    public override string ToString()
    {
        return Value;
    }

    public int CompareTo(object? obj)
    {
        return string.Compare(Value, ((CombatantStatusSid?)obj)?.Value, StringComparison.InvariantCultureIgnoreCase);
    }
}