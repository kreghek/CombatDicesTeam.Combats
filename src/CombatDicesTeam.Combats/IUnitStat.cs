namespace CombatDicesTeam.Combats;

public interface ICombatantStat
{
    ICombatantStatType Type { get; }
    IStatValue Value { get; }
    event EventHandler<CombatantStatChangedEventArgs>? Changed;
}

public sealed class CombatantStatChangedEventArgs : EventArgs
{
    public ICombatantStatType StatType { get; }
    public int Amount { get; }

    public CombatantStatChangedEventArgs(ICombatantStatType statType, int amount)
    {
        StatType = statType;
        Amount = amount;
    }
}