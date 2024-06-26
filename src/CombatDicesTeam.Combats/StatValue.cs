namespace CombatDicesTeam.Combats;

public class StatValue : IStatValue
{
    private readonly IList<IStatModifier> _modifiers;

    public StatValue(int baseValue)
    {
        Base = baseValue;
        Current = Base;
        _modifiers = new List<IStatModifier>();
    }

    private int Base { get; set; }

    public int ActualMax => Math.Max(0, Base + _modifiers.Sum(x => x.Value));

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
        Base = newBase;
    }

    public void Consume(int amount)
    {
        Current -= amount;

        if (Current < 0)
        {
            Current = 0;
        }
    }

    public void CurrentChange(int newCurrent)
    {
        Current = Math.Min(newCurrent, ActualMax);
    }

    public void Restore(int amount)
    {
        Current += amount;

        if (Current > ActualMax)
        {
            Current = ActualMax;
        }
    }

    public void RemoveModifier(IStatModifier modifier)
    {
        _modifiers.Remove(modifier);

        if (Current > ActualMax)
        {
            Current = ActualMax;
        }
    }

    public event EventHandler? ModifierAdded;
}