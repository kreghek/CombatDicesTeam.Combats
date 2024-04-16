using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public record CombatantStatusSid(string Value) : ICombatantStatusSid
{
    public override string ToString()
    {
        return Value;
    }
}