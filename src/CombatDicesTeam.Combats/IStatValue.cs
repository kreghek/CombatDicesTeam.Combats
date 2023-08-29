namespace CombatDicesTeam.Combats;

public interface IStatValue
{
    int ActualMax { get; }
    int Current { get; }

    /// <summary>
    /// Current value modifiers to handle or display.
    /// </summary>
    IReadOnlyCollection<IStatModifier> Modifiers { get; }

    void AddModifier(IStatModifier modifier);
    void ChangeBase(int newBase);
    void Consume(int value);
    void CurrentChange(int newCurrent);
    void RemoveModifier(IStatModifier modifier);
    void Restore(int value);

    event EventHandler? ModifierAdded;
}