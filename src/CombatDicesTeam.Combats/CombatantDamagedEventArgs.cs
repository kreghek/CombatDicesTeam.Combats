namespace CombatDicesTeam.Combats;

public sealed class CombatantDamagedEventArgs : EventArgs
{
    public CombatantDamagedEventArgs(ICombatant combatant, ICombatantStatType statType, int value)
    {
        Combatant = combatant;
        StatType = statType;
        Value = value;
    }

    public ICombatant Combatant { get; }
    public ICombatantStatType StatType { get; }
    public int Value { get; }
}