using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public sealed record TargetSelectorContext
    (CombatFieldSide ActorSide, CombatFieldSide EnemySide, IDice Dice) : ITargetSelectorContext;