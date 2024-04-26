namespace CombatDicesTeam.Combats;

public delegate int CombatantHasTakenDamagedCallback(
    ICombatant targetCombatant,
    IDamageSource damageSource,
    ICombatantStatType damagedStat,
    StatDamage damage);