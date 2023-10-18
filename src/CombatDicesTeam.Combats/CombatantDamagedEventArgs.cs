using JetBrains.Annotations;

using static CombatDicesTeam.Combats.CombatEngineBase;

namespace CombatDicesTeam.Combats;

public sealed class CombatantDamagedEventArgs : EventArgs
{
    public CombatantDamagedEventArgs(ICombatant combatant, ICombatantStatType statType, StatDamage damage)
    {
        Combatant = combatant;
        StatType = statType;
        Damage = damage;
    }

    [PublicAPI]
    public ICombatant Combatant { get; }

    [PublicAPI]
    public ICombatantStatType StatType { get; }

    [PublicAPI]
    public StatDamage Damage { get; }
}