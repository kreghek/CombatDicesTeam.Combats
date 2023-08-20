using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public record CombatantEffectSid(string Value) : ICombatantStatusSid, IComparable
{
    public override string ToString()
    {
        return Value;
    }

    public int CompareTo(object? obj)
    {
        return string.Compare(Value, ((CombatantEffectSid?)obj)?.Value, StringComparison.InvariantCultureIgnoreCase);
    }
}