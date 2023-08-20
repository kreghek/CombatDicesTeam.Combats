using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

public sealed class CombatantDamagedEventArgs : EventArgs
{
    public CombatantDamagedEventArgs(ICombatant combatant, ICombatantStatType statType, int value)
    {
        Combatant = combatant;
        StatType = statType;
        Value = value;
    }

    [PublicAPI]
    public ICombatant Combatant { get; }

    [PublicAPI]
    public ICombatantStatType StatType { get; }

    [PublicAPI]
    public int Value { get; }
}