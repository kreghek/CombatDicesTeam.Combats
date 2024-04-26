using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed class CombatantDamagedEventArgs : EventArgs
{
    [PublicAPI]
    public IDamageSource DamageSource { get; }

    public CombatantDamagedEventArgs(ICombatant combatant, IDamageSource damageSource, ICombatantStatType statType,
        StatDamage damage)
    {
        DamageSource = damageSource;
        Combatant = combatant;
        StatType = statType;
        Damage = damage;
    }

    [PublicAPI]
    public ICombatant Combatant { get; }

    [PublicAPI]
    public StatDamage Damage { get; }

    [PublicAPI]
    public ICombatantStatType StatType { get; }
}