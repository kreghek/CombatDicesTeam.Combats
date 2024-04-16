using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public interface ICombatMovementContext
{
    ICombatant Actor { get; }
    IDice Dice { get; }
    CombatField Field { get; }

    ICombatantStatusImposeContext StatusImposedContext { get; }
    ICombatantStatusLifetimeImposeContext StatusLifetimeImposedContext { get; }

    int DamageCombatantStat(ICombatant combatant, ICombatantStatType statType, StatDamage damage);

    void NotifySwapFieldPosition(ICombatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide, IPositionChangingReason moveReason);

    void PassTurn(ICombatant target);
    void RestoreCombatantStat(ICombatant combatant, ICombatantStatType statType, int value);
}