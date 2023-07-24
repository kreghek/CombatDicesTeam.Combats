namespace CombatDicesTeam.Utils;

public sealed record Range<T>(T Min, T Max)
{
    public static Range<T> CreateMono(T Value)
    {
        return new Range<T>(Value, Value);
    }
}