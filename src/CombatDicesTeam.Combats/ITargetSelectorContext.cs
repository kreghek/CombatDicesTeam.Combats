using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

/// <summary>
/// The context to get world info in the combat move target selector.
/// </summary>
public interface ITargetSelectorContext
{
    CombatFieldSide ActorSide { get; }
    IDice Dice { get; }
    CombatFieldSide EnemySide { get; }
    IReadOnlyCollection<ICombatant> GetCombatants(ITargetSelectorContextCombatantType combatantType);
}