using JetBrains.Annotations;

namespace CombatDicesTeam.Combats.Effects;

public sealed record CounterCombatantStatusSource([UsedImplicitly] ICombatantStatus Status) : ICombatantStatusSource;