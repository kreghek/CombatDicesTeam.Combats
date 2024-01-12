namespace CombatDicesTeam.Combats;

public sealed class StatModifier : IStatModifier
{
    private readonly Func<int> _valueCalculator;

    public StatModifier(int fixedValue, IStatModifierSource source) : this(() => fixedValue, source) { }

    public StatModifier(Func<int> valueCalculator, IStatModifierSource source)
    {
        _valueCalculator = valueCalculator;
        Source = source;
    }

    public int Value => _valueCalculator();

    public IStatModifierSource Source { get; }
}