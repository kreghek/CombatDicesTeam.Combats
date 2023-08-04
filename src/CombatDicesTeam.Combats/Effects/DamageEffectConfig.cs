using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.Effects;

[PublicAPI]
public sealed record DamageEffectConfig(ICombatantStatType MainStatType, ICombatantStatType ProtectionStatType,
    ICombatantStatType AbsorptionStatType);