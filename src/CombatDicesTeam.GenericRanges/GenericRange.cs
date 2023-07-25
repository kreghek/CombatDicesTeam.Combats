namespace CombatDicesTeam.GenericRanges;

public sealed record GenericRange<T>(T Min, T Max)
{
    public static GenericRange<T> CreateMono(T Value)
    {
        return new GenericRange<T>(Value, Value);
    }
}