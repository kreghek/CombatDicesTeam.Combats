namespace CombatDicesTeam.Combats;

public delegate int CombatantHasTakenDamagedCallback(ICombatant targetCombatant, ICombatantStatType damagedStat,
    StatDamage Damage);