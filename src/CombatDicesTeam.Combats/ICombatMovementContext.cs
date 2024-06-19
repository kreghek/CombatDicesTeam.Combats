using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public interface ICombatMovementContext
{
    ICombatant Actor { get; }
    IDice Dice { get; }
    CombatField Field { get; }

    ICombatantStatusImposeContext StatusImposedContext { get; }
    ICombatantStatusLifetimeImposeContext StatusLifetimeImposedContext { get; }

    void ChangeCombatStat(ICombatant target, IStatChangingSource damageSource, ICombatantStatType statType, int amount);

    int DamageCombatantStat(ICombatant target, IStatChangingSource damageSource, ICombatantStatType statType,
        StatDamage damage);

    void MoveToPosition(ICombatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide, IPositionChangingReason moveReason);

    void PassTurn(ICombatant target);
}