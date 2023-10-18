using static CombatDicesTeam.Combats.CombatEngineBase;

namespace CombatDicesTeam.Combats;

public delegate int CombatantHasTakenDamagedCallback(ICombatant targetCombatant, ICombatantStatType damagedStat,
    StatDamage Damage);