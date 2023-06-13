namespace CombatDicesTeam.Combats;

public interface ICombatant
{
    bool IsInactive { get; }

    ITeam Team { get; }

    void UpdateEffects(CombatantEffectUpdateType updateType,
        ICombatantEffectLifetimeDispelContext effectLifetimeDispelContext);

    IReadOnlyCollection<IStat> Stats { get; }
}

public interface IStat
{
    IStatType Type { get; }
    IStatValue Value { get; }
}

public interface IStatType
{
}

public interface IStatValue
{
    int ActualMax { get; }
    int Current { get; }
    void AddModifier(IStatModifier modifier);
    void ChangeBase(int newBase);
    void Consume(int value);
    void CurrentChange(int newCurrent);
    void RemoveModifier(IStatModifier modifier);
    void Restore(int value);

    event EventHandler? ModifierAdded;
}

public interface IStatModifier
{
    int Value { get; }
}