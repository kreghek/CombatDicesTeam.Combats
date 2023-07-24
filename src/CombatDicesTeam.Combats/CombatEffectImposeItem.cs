namespace Core.Combats;

public record CombatEffectImposeItem(CombatEffectImposeDelegate ImposeDelegate,
    IReadOnlyList<ICombatant> MaterializedTargets);