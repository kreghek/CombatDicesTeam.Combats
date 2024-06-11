namespace CombatDicesTeam.Combats;

public sealed class StatChangedEventArgs : EventArgs
{
    public int Amount { get; }

    public StatChangedEventArgs(int amount)
    {
        Amount = amount;
    }
}