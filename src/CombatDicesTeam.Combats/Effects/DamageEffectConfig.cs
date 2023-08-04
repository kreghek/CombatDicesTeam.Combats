namespace CombatDicesTeam.Combats.Effects;

public sealed record DamageEffectConfig(ICombatantStatType MainStatType, ICombatantStatType ProtectionStatType,
    ICombatantStatType AbsorptionStatType);