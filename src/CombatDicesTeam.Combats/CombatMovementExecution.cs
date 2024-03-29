﻿namespace CombatDicesTeam.Combats;

public record CombatMovementExecution(
    CombatMovementCompleteCallback CompleteDelegate,
    IReadOnlyCollection<CombatEffectImposeItem> EffectImposeItems);