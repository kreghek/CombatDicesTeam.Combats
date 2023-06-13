namespace CombatDicesTeam.Combats;

public sealed record CombatMovementSid(string Value)
{
    public static implicit operator CombatMovementSid(string source)
    {
        return new CombatMovementSid(source);
    }

    public static implicit operator string(CombatMovementSid source)
    {
        return source.Value;
    }
}