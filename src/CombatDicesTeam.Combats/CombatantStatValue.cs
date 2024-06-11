using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed class CombatantStatValue : IStatValue
{
    private readonly IStatValue _baseValue;

    private readonly IList<IStatModifier> _modifiers;

    public CombatantStatValue(IStatValue baseValue)
    {
        _modifiers = new List<IStatModifier>();

        _baseValue = baseValue;

        Current = ActualMax;

        _baseValue.ModifierAdded += BaseValue_ModifierAdded;
        _baseValue.CurrentChanged += BaseValue_CurrentChanged;
    }

    private void BaseValue_CurrentChanged(object? sender, StatChangedEventArgs e)
    {
        CurrentChanged?.Invoke(this, e);
    }

    private void BaseValue_ModifierAdded(object? sender, EventArgs e)
    {
        if (Current > ActualMax)
        {
            Current = ActualMax;
        }
    }

    public int ActualMax => _baseValue.ActualMax + _modifiers.Sum(x => x.Value);

    public int Current { get; private set; }
    public IReadOnlyCollection<IStatModifier> Modifiers => _modifiers.ToArray();

    public void AddModifier(IStatModifier modifier)
    {
        _modifiers.Add(modifier);

        if (Current > ActualMax)
        {
            Current = ActualMax;
        }

        ModifierAdded?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeBase(int newBase)
    {
        _baseValue.ChangeBase(newBase);
    }

    public void Consume(int value)
    {
        Current -= value;

        if (Current < 0)
        {
            Current = 0;
        }
    }

    public void CurrentChange(int newCurrent)
    {
        Current = Math.Min(newCurrent, ActualMax);
    }

    public void RemoveModifier(IStatModifier modifier)
    {
        _modifiers.Remove(modifier);
    }

    public void Restore(int value)
    {
        Current += value;

        if (Current > ActualMax)
        {
            Current = ActualMax;
        }
    }

    public event EventHandler? ModifierAdded;
    public event EventHandler<StatChangedEventArgs>? CurrentChanged;
}