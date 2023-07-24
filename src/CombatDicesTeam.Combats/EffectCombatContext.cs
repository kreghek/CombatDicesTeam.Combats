using CombatDicesTeam.Dices;

namespace Core.Combats;

public sealed class EffectCombatContext : IStatusCombatContext
{
    public EffectCombatContext(
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

        StatusImposedContext = new CombatantEffectImposeContext(combatCore);
        StatusLifetimeImposedContext = new CombatantEffectLifetimeImposeContext(targetCombatant, combatCore);
    }

    public CombatantHasTakenDamagedCallback NotifyCombatantDamagedDelegate { get; }
    public CombatantHasMovedCallback NotifyCombatantMovedDelegate { get; }

    public ICombatant Actor { get; }

    public int DamageCombatantStat(ICombatant combatant, ICombatantStatType statType, int value)
    {
        return NotifyCombatantDamagedDelegate(combatant, statType, value);
    }

    public void NotifySwapFieldPosition(ICombatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide)
    {
        NotifyCombatantMovedDelegate(sourceCoords, sourceFieldSide, destinationCoords, destinationFieldSide);
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