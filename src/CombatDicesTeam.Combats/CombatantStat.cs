namespace CombatDicesTeam.Combats;

public sealed class CombatantStat : ICombatantStat
{
    public CombatantStat(ICombatantStatType type, IStatValue value)
    {
        Type = type;
        Value = value;

        Value.CurrentChanged += (_, args) =>
        {
            Changed?.Invoke(this, new CombatantStatChangedEventArgs(type, args.Amount));
        };
    }

    public ICombatantStatType Type { get; }
    public IStatValue Value { get; }
    public event EventHandler<CombatantStatChangedEventArgs>? Changed;
}