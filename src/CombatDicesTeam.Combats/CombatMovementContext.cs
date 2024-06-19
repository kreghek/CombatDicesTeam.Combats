using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public sealed class CombatMovementContext : ICombatMovementContext
{
    private readonly CombatEngineBase _combatCore;

    public CombatMovementContext(
        ICombatant movementActor,
        CombatField field,
        IDice dice,
        CombatEngineBase combatCore)
    {
        _combatCore = combatCore;
        
        Actor = movementActor;
        Field = field;
        Dice = dice;

        StatusImposedContext = new CombatantStatusImposeContext(combatCore);
        StatusLifetimeImposedContext = new CombatantStatusLifetimeImposeContext(movementActor, combatCore);
    }

    public ICombatant Actor { get; }

    public int DamageCombatantStat(ICombatant target, IStatChangingSource damageSource, ICombatantStatType statType,
        StatDamage damage)
    {
        return _combatCore.HandleCombatantDamagedToStat(target, damageSource, statType, damage);
    }

    public void ChangeCombatStat(ICombatant target, IStatChangingSource damageSource, ICombatantStatType statType, int amount)
    {
        _combatCore.ChangeCombatantStat(target, damageSource, statType, amount);
    }

    public void MoveToPosition(ICombatant combatant, FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide, IPositionChangingReason moveReason)
    {
        _combatCore.HandleSwapFieldPositions(sourceCoords, sourceFieldSide, destinationCoords, destinationFieldSide,
            moveReason);
    }

    public void PassTurn(ICombatant target)
    {
    }

    public ICombatantStatusLifetimeImposeContext StatusLifetimeImposedContext { get; }

    public ICombatantStatusImposeContext StatusImposedContext { get; }

    public CombatField Field { get; }
    public IDice Dice { get; }
}