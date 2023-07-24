using Core.Dices;

namespace Core.Combats;

public sealed record TargetSelectorContext
    (CombatFieldSide ActorSide, CombatFieldSide EnemySide, IDice Dice) : ITargetSelectorContext;