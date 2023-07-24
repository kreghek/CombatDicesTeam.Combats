using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public interface ITargetSelectorContext
{
    CombatFieldSide ActorSide { get; }
    IDice Dice { get; }
    CombatFieldSide EnemySide { get; }
}