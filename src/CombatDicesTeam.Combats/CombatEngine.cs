using System.Collections.ObjectModel;

namespace CombatDicesTeam.Combats;

public class CombatEngine
{
    private readonly IList<ICombatant> _allCombatantList;

    private readonly IList<ICombatant> _roundQueue;

    public CombatEngine(ICombatField combatField)
    {
        Field = combatField;

        _allCombatantList = new Collection<ICombatant>();
        _roundQueue = new List<ICombatant>();
    }

    public IReadOnlyCollection<ICombatant> Combatants => _allCombatantList.ToArray();

    public ICombatant CurrentCombatant => _roundQueue.FirstOrDefault() ?? throw new InvalidOperationException();

    public ICombatField Field { get; }

    public bool Finished
    {
        get
        {
            var aliveUnits = _allCombatantList.Where(x => !x.IsInactive).ToArray();

            return aliveUnits.All(x => x.Team == aliveUnits.First().Team);
        }
    }

    public IReadOnlyList<ICombatant> RoundQueue => _roundQueue.ToArray();

    public void CompleteTurn()
    {
        CombatantEndsTurn?.Invoke(this, new CombatantEndsTurnEventArgs(CurrentCombatant));
        
        var context = new CombatantEffectLifetimeDispelContext(this);

        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.EndCombatantTurn, context);

        if (_roundQueue.Any()) RemoveCurrentCombatantFromRoundQueue();

        while (true)
        {
            if (!_roundQueue.Any())
            {
                UpdateAllCombatantEffects(CombatantEffectUpdateType.EndRound, context);

                if (Finished)
                {
                    var combatResult = CalcResult();
                    CombatFinished?.Invoke(this, new CombatFinishedEventArgs(combatResult));
                    return;
                }

                StartRound(context);

                CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));

                return;
            }

            if (_roundQueue.First().IsInactive)
            {
                RemoveCurrentCombatantFromRoundQueue();
            }
            else
            {
                if (Finished)
                {
                    var combatResult = CalcResult();
                    CombatFinished?.Invoke(this, new CombatFinishedEventArgs(combatResult));
                    return;
                }

                break;
            }
        }

        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.StartCombatantTurn, context);

        CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));
    }

    public CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement)
    {
        SpendStatsOnCombatMovement();

        var handSlotIndex = CurrentCombatant.DropMovementFromHand(movement);

        if (handSlotIndex is not null)
            CombatantUsedMove?.Invoke(this,
                new CombatantHandChangedEventArgs(CurrentCombatant, movement, handSlotIndex.Value));

        var effectContext =
            new EffectCombatContext(Field, _dice, HandleCombatantDamagedToStat, HandleSwapFieldPositions, this);

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effectInstance in movement.Effects)
        {
            void EffectInfluenceDelegate(Combatant materializedTarget)
            {
                effectInstance.Influence(materializedTarget, effectContext);
            }

            var effectTargets = effectInstance.Selector.GetMaterialized(CurrentCombatant, GetCurrentSelectorContext());

            if (movement.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack))
                foreach (var effectTarget in effectTargets)
                {
                    if (effectTarget == CurrentCombatant)
                        // Does not defence against yourself.
                        continue;

                    var targetDefenseMovement = GetAutoDefenseMovement(effectTarget);
                    var targetIsInQueue = _roundQueue.Any(x => x == effectTarget);

                    if (targetDefenseMovement is not null && targetIsInQueue)
                    {
                        foreach (var autoDefenseEffect in targetDefenseMovement.AutoDefenseEffects)
                        {
                            void AutoEffectInfluenceDelegate(Combatant materializedTarget)
                            {
                                autoDefenseEffect.Influence(materializedTarget, effectContext);
                            }

                            var autoDefenseEffectTargets =
                                effectInstance.Selector.GetMaterialized(effectTarget, GetSelectorContext(effectTarget));

                            var autoDefenseEffectImposeItem =
                                new CombatEffectImposeItem(AutoEffectInfluenceDelegate, autoDefenseEffectTargets);

                            effectImposeItems.Add(autoDefenseEffectImposeItem);
                        }

                        _roundQueue.Remove(effectTarget);
                        var autoHandSlotIndex = effectTarget.DropMovementFromHand(targetDefenseMovement);

                        if (autoHandSlotIndex is not null)
                            CombatantUsedMove?.Invoke(this,
                                new CombatantHandChangedEventArgs(effectTarget, targetDefenseMovement,
                                    autoHandSlotIndex.Value));
                    }
                }

            var effectImposeItem = new CombatEffectImposeItem(EffectInfluenceDelegate, effectTargets);

            // Play auto-defence effects before an attacks.
            effectImposeItems.Add(effectImposeItem);
        }

        void CompleteSkillAction()
        {
        }

        var movementExecution = new CombatMovementExecution(CompleteSkillAction, effectImposeItems);

        return movementExecution;
    }

    private void SpendStatsOnCombatMovement()
    {
        CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Resolve).Value.Consume(1);
    }

    public ITargetSelectorContext GetCurrentSelectorContext()
    {
        return GetSelectorContext(CurrentCombatant);
    }

    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, Field.HeroSide);
        InitializeCombatFieldSide(monsters, Field.MonsterSide);

        foreach (var combatant in _allCombatantList) combatant.PrepareToCombat();

        var context = new CombatantEffectLifetimeDispelContext(this);
        StartRound(context);

        CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));
    }

    public void Interrupt()
    {
        CombatantInterrupted?.Invoke(this, new CombatantInterruptedEventArgs(CurrentCombatant));
        CompleteTurn();
    }

    public void UseManeuver(CombatStepDirection combatStepDirection)
    {
        var currentCoords = GetCurrentCoords();

        var targetCoords = combatStepDirection switch
        {
            CombatStepDirection.Up => currentCoords with
            {
                LineIndex = currentCoords.LineIndex - 1
            },
            CombatStepDirection.Down => currentCoords with
            {
                LineIndex = currentCoords.LineIndex + 1
            },
            CombatStepDirection.Forward => currentCoords with
            {
                ColumentIndex = currentCoords.ColumentIndex - 1
            },
            CombatStepDirection.Backward => currentCoords with
            {
                ColumentIndex = currentCoords.ColumentIndex + 1
            },
            _ => throw new ArgumentOutOfRangeException(nameof(combatStepDirection), combatStepDirection, null)
        };

        var side = GetCurrentSelectorContext().ActorSide;

        HandleSwapFieldPositions(currentCoords, side, targetCoords, side);

        CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Maneuver).Value.Consume(1);
    }

    public void Wait()
    {
        RestoreStatOfAllCombatants(UnitStatType.Resolve);

        CompleteTurn();
    }

    private CombatFinishResult CalcResult()
    {
        var aliveUnits = _allCombatantList.Where(x => !x.IsInactive).ToArray();
        var playerUnits = aliveUnits.Where(x => x.IsPlayerControlled);
        var hasPlayerUnits = playerUnits.Any();

        var cpuUnits = aliveUnits.Where(x => !x.IsPlayerControlled);
        var hasCpuUnits = cpuUnits.Any();

        // TODO Looks like XOR
        if (hasPlayerUnits && !hasCpuUnits) return CombatFinishResult.HeroesAreWinners;

        if (!hasPlayerUnits && hasCpuUnits) return CombatFinishResult.MonstersAreWinners;

        return CombatFinishResult.Draw;
    }

    private bool DetectShapeShifting()
    {
        return false;
    }

    private void DoCombatantHasBeenAdded(CombatFieldSide targetSide, FieldCoords targetCoords, Combatant combatant)
    {
        var combatFieldInfo = new CombatFieldInfo(targetSide, targetCoords);
        var args = new CombatantHasBeenAddedEventArgs(combatant, combatFieldInfo);
        CombatantHasBeenAdded?.Invoke(this, args);
    }

    private static CombatMovementInstance? GetAutoDefenseMovement(Combatant target)
    {
        return target.Hand.FirstOrDefault(x =>
            x != null && x.SourceMovement.Tags.HasFlag(CombatMovementTags.AutoDefense));
    }

    private FieldCoords GetCurrentCoords()
    {
        var side = GetCurrentSelectorContext().ActorSide;

        for (var col = 0; col < side.ColumnCount; col++)
        for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
            if (CurrentCombatant == side[new FieldCoords(col, lineIndex)].Combatant)
                return new FieldCoords(col, lineIndex);

        throw new InvalidOperationException();
    }

    private ITargetSelectorContext GetSelectorContext(Combatant combatant)
    {
        if (combatant.IsPlayerControlled)
            return new TargetSelectorContext(Field.HeroSide, Field.MonsterSide, _dice);
        return new TargetSelectorContext(Field.MonsterSide, Field.HeroSide, _dice);
    }

    private static CombatFieldSide GetTargetSide(Combatant target, CombatField field)
    {
        try
        {
            var _ = field.HeroSide.GetCombatantCoords(target);
            return field.HeroSide;
        }
        catch (ArgumentException)
        {
            var _ = field.MonsterSide.GetCombatantCoords(target);
            return field.MonsterSide;
        }
    }

    private int HandleCombatantDamagedToStat(Combatant combatant, UnitStatType statType, int damageAmount)
    {
        var (remains, wasTaken) = TakeStat(combatant, statType, damageAmount);

        if (wasTaken)
            CombatantHasBeenDamaged?.Invoke(this, new CombatantDamagedEventArgs(combatant, statType, damageAmount));

        if (combatant.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.Current <= 0)
        {
            var shiftShape = DetectShapeShifting();
            if (shiftShape) CombatantShiftShaped?.Invoke(this, new CombatantShiftShapedEventArgs(combatant));

            combatant.SetDead();
            CombatantHasBeenDefeated?.Invoke(this, new CombatantDefeatedEventArgs(combatant));

            var targetSide = GetTargetSide(combatant, Field);
            var coords = targetSide.GetCombatantCoords(combatant);
            targetSide[coords].Combatant = null;
        }

        return remains;
    }

    private void HandleSwapFieldPositions(FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide)
    {
        if (sourceCoords == destinationCoords && sourceFieldSide == destinationFieldSide) return;

        var sourceCombatant = sourceFieldSide[sourceCoords].Combatant;
        var targetCombatant = destinationFieldSide[destinationCoords].Combatant;

        destinationFieldSide[destinationCoords].Combatant = sourceCombatant;

        if (sourceCombatant is not null)
            CombatantHasBeenMoved?.Invoke(this,
                new CombatantHasBeenMovedEventArgs(sourceCombatant, destinationFieldSide, destinationCoords));

        sourceFieldSide[sourceCoords].Combatant = targetCombatant;

        if (targetCombatant is not null)
            CombatantHasBeenMoved?.Invoke(this,
                new CombatantHasBeenMovedEventArgs(targetCombatant, sourceFieldSide, sourceCoords));
    }

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> formationSlots, CombatFieldSide side)
    {
        foreach (var slot in formationSlots)
        {
            if (slot.Combatant is null) continue;

            var coords = new FieldCoords(slot.ColumnIndex, slot.LineIndex);
            side[coords].Combatant = slot.Combatant;

            _allCombatantList.Add(slot.Combatant);

            DoCombatantHasBeenAdded(targetSide: side, targetCoords: coords, slot.Combatant);
        }
    }

    private void MakeUnitRoundQueue()
    {
        _roundQueue.Clear();

        var orderedByResolve = _allCombatantList
            .Where(x => !x.IsInactive)
            .OrderByDescending(x => x.Stats.Single(s => s.Type == UnitStatType.Resolve).Value.Current)
            .ThenByDescending(x => x.IsPlayerControlled)
            .ToArray();

        foreach (var unit in orderedByResolve)
            if (!unit.IsInactive)
                _roundQueue.Add(unit);
    }

    private void RemoveCurrentCombatantFromRoundQueue()
    {
        _roundQueue.RemoveAt(0);
    }

    private void RestoreCombatantHand(Combatant combatant)
    {
        for (var handSlotIndex = 0; handSlotIndex < combatant.Hand.Count; handSlotIndex++)
            if (combatant.Hand[handSlotIndex] is null)
            {
                var nextMove = combatant.PopNextPoolMovement();
                if (nextMove is null) break;

                combatant.AssignMoveToHand(handSlotIndex, nextMove);
                CombatantAssignedNewMove?.Invoke(this,
                    new CombatantHandChangedEventArgs(combatant, nextMove, handSlotIndex));
            }
    }

    private void RestoreHandsOfAllCombatants()
    {
        foreach (var combatant in _allCombatantList) RestoreCombatantHand(combatant);
    }

    private void RestoreManeuversOfAllCombatants()
    {
        RestoreStatOfAllCombatants(UnitStatType.Maneuver);
    }

    private void RestoreShieldsOfAllCombatants()
    {
        RestoreStatOfAllCombatants(UnitStatType.ShieldPoints);
    }

    private void RestoreStatOfAllCombatants(UnitStatType statType)
    {
        var combatants = _allCombatantList.Where(x => !x.IsInactive);
        foreach (var combatant in combatants)
        {
            var stat = combatant.Stats.Single(x => x.Type == statType);
            var valueToRestore = stat.Value.ActualMax - stat.Value.Current;
            stat.Value.Restore(valueToRestore);
        }
    }

    private void StartRound(ICombatantEffectLifetimeDispelContext combatantEffectLifetimeDispelContext)
    {
        MakeUnitRoundQueue();
        RestoreHandsOfAllCombatants();
        RestoreShieldsOfAllCombatants();
        RestoreManeuversOfAllCombatants();

        UpdateAllCombatantEffects(CombatantEffectUpdateType.StartRound, combatantEffectLifetimeDispelContext);
        CurrentCombatant.UpdateEffects(CombatantEffectUpdateType.StartCombatantTurn,
            combatantEffectLifetimeDispelContext);
    }

    private static (int result, bool isTaken) TakeStat(Combatant combatant, UnitStatType statType, int value)
    {
        var stat = combatant.Stats.SingleOrDefault(x => x.Type == statType);

        if (stat is null) return (value, false);

        var d = Math.Min(value, stat.Value.Current);
        stat.Value.Consume(d);

        var remains = value - d;

        return (remains, d > 0);
    }

    private void UpdateAllCombatantEffects(CombatantEffectUpdateType updateType,
        ICombatantEffectLifetimeDispelContext context)
    {
        foreach (var combatant in _allCombatantList)
            if (!combatant.IsInactive)
                combatant.UpdateEffects(updateType, context);
    }

    public event EventHandler<CombatantHasBeenAddedEventArgs>? CombatantHasBeenAdded;
    public event EventHandler<CombatantTurnStartedEventArgs>? CombatantStartsTurn;
    public event EventHandler<CombatantEndsTurnEventArgs>? CombatantEndsTurn;
    public event EventHandler<CombatantDamagedEventArgs>? CombatantHasBeenDamaged;
    public event EventHandler<CombatantDefeatedEventArgs>? CombatantHasBeenDefeated;
    public event EventHandler<CombatantShiftShapedEventArgs>? CombatantShiftShaped;
    public event EventHandler<CombatantHasBeenMovedEventArgs>? CombatantHasBeenMoved;
    public event EventHandler<CombatFinishedEventArgs>? CombatFinished;
    public event EventHandler<CombatantInterruptedEventArgs>? CombatantInterrupted;
    public event EventHandler<CombatantHandChangedEventArgs>? CombatantAssignedNewMove;
    public event EventHandler<CombatantHandChangedEventArgs>? CombatantUsedMove;
}

public sealed class CombatMovementInstance
{
    public CombatMovementInstance(CombatMovement sourceMovement)
    {
        SourceMovement = sourceMovement;
        Effects = sourceMovement.Effects.Select(x => x.CreateInstance()).ToArray();
        AutoDefenseEffects = sourceMovement.AutoDefenseEffects.Select(x => x.CreateInstance()).ToArray();

        Cost = new CombatMovementCost(new StatValue(sourceMovement.Cost.Amount.Current));
    }

    public IReadOnlyCollection<IEffectInstance> AutoDefenseEffects { get; }

    public CombatMovementCost Cost { get; }

    public IReadOnlyCollection<IEffectInstance> Effects { get; }

    public CombatMovement SourceMovement { get; }
}

public sealed class CombatMovement
{
    public CombatMovement(CombatMovementSid sid, CombatMovementCost cost, CombatMovementEffectConfig effectConfig)
    {
        Sid = sid;
        Cost = cost;
        Effects = effectConfig.Effects;
        AutoDefenseEffects = effectConfig.AutoDefenseEffects;
    }

    public IReadOnlyCollection<IEffect> AutoDefenseEffects { get; }
    public CombatMovementCost Cost { get; }
    public IReadOnlyCollection<IEffect> Effects { get; }
    public CombatMovementSid Sid { get; }
    public CombatMovementTags Tags { get; init; }
}

public interface IEffect
{
    IReadOnlyCollection<IEffectCondition> ImposeConditions { get; }
    ITargetSelector Selector { get; }

    IEffectInstance CreateInstance();
}

public interface ITargetSelector
{
    /// <summary>
    /// Get estimate target to visualize before execution.
    /// </summary>
    IReadOnlyList<CombatMoveTargetEstimate> GetEstimate(ICombatant actor, ITargetSelectorContext context)
    {
        return GetMaterialized(actor, context)
            .Select(x => new CombatMoveTargetEstimate(x, CombatMoveTargetEstimateType.Exactly))
            .ToArray();
    }

    IReadOnlyList<Combatant> GetMaterialized(Combatant actor, ITargetSelectorContext context);
}

public sealed record CombatMoveTargetEstimate(ICombatant Target, CombatMoveTargetEstimateType EstimateType);

public interface ITargetSelectorContext
{
    CombatFieldSide ActorSide { get; }
    IDice Dice { get; }
    CombatFieldSide EnemySide { get; }
}