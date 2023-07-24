using CombatDicesTeam.Dices;

namespace Core.Combats;

public interface ITargetSelectorContext
{
    CombatFieldSide ActorSide { get; }
    IDice Dice { get; }
    CombatFieldSide EnemySide { get; }
}