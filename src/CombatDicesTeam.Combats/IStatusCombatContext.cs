using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public interface IStatusCombatContext
{
    ICombatant Actor { get; }
    IDice Dice { get; }
    CombatField Field { get; }

    ICombatantStatusImposeContext StatusImposedContext { get; }
    ICombatantStatusLifetimeImposeContext StatusLifetimeImposedContext { get; }

    int DamageCombatantStat(ICombatant combatant, ICombatantStatType statType, int value);

    void NotifySwapFieldPosition(ICombatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide);

    void PassTurn(ICombatant target);
    void RestoreCombatantStat(ICombatant combatant, ICombatantStatType statType, int value);
}