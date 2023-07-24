namespace Core.Combats;

// ReSharper disable once InconsistentNaming
public static class IStatValueExtensions
{
    public static double GetShare(this IStatValue source)
    {
        return (double)source.Current / source.ActualMax;
    }

    public static void Restore(this IStatValue source)
    {
        source.Restore(source.ActualMax);
    }
}