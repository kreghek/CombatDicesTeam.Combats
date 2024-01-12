using CombatDicesTeam.Dices;

using static CombatDicesTeam.Combats.CombatEngineBase;

namespace CombatDicesTeam.Combats;

public sealed class StatusCombatContext : ICombatMovementContext
{
    public StatusCombatContext(
        ICombatant targetCombatant,
        CombatField field,
        IDice dice,
        CombatantHasTakenDamagedCallback notifyCombatantDamagedDelegate,
        CombatantHasMovedCallback notifyCombatantMovedDelegate,
        CombatEngineBase combatCore)
    {
        Actor = targetCombatant;
        Field = field;
        Dice = dice;
        NotifyCombatantDamagedDelegate = notifyCombatantDamagedDelegate;
        NotifyCombatantMovedDelegate = notifyCombatantMovedDelegate;

        StatusImposedContext = new CombatantStatusImposeContext(combatCore);
        StatusLifetimeImposedContext = new CombatantStatusLifetimeImposeContext(targetCombatant, combatCore);
    }

    public CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate { get; }
    public CombatantHasMovedCallback NotifyCombatantMovedDelegate { get; }

    public ICombatant Actor { get; }

    public int DamageCombatantStat(ICombatant combatant, ICombatantStatType statType, StatDamage damage)
    {
        return NotifyCombatantDamagedDelegate(combatant, statType, damage);
    }

    public void NotifySwapFieldPosition(ICombatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide, IPositionChangingReason moveReason)
    {
        NotifyCombatantMovedDelegate(sourceCoords, sourceFieldSide, destinationCoords, destinationFieldSide,
            moveReason);
    }

    public void PassTurn(ICombatant target)
    {
    }

    public void RestoreCombatantStat(ICombatant combatant, ICombatantStatType statType, int value)
    {
        throw new NotImplementedException();
    }

    public ICombatantStatusLifetimeImposeContext StatusLifetimeImposedContext { get; }

    public ICombatantStatusImposeContext StatusImposedContext { get; }

    public CombatField Field { get; }
    public IDice Dice { get; }
}