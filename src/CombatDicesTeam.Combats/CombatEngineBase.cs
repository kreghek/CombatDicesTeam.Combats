using System.Collections.ObjectModel;

using CombatDicesTeam.Dices;

using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

public abstract class CombatEngineBase
{
    private readonly IList<ICombatant> _roundQueue;

    private readonly IRoundQueueResolver _roundQueueResolver;

    [PublicAPI] protected readonly IList<ICombatant> AllCombatantList;

    [PublicAPI] protected readonly IDice Dice;

    protected CombatEngineBase(IDice dice, IRoundQueueResolver roundQueueResolver, ICombatStateStrategy stateStrategy)
    {
        Dice = dice;
        _roundQueueResolver = roundQueueResolver;
        StateStrategy = stateStrategy;
        Field = new CombatField();

        AllCombatantList = new Collection<ICombatant>();
        _roundQueue = new List<ICombatant>();

        CurrentRoundNumber = 1;
    }

    /// <summary>
    /// Current active combatant.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public ICombatant CurrentCombatant => _roundQueue.FirstOrDefault() ?? throw new InvalidOperationException();

    /// <summary>
    /// All combatants in the combat.
    /// </summary>
    /// <remarks>
    /// Combat queue will create based on this list.
    /// </remarks>
    [PublicAPI]
    public IReadOnlyCollection<ICombatant> CurrentCombatants => AllCombatantList.ToArray();

    /// <summary>
    /// Current combat round.
    /// </summary>
    /// <remarks>
    /// You can use it to:
    /// - Display round number in your game client.
    /// - Override conditions to finish combat based on round number.
    /// </remarks>
    public int CurrentRoundNumber { get; private set; }

    /// <summary>
    /// Current combat queue of turns.
    /// </summary>
    [PublicAPI]
    public IReadOnlyList<ICombatant> CurrentRoundQueue => _roundQueue.ToArray();

    /// <summary>
    /// Combat field.
    /// </summary>
    [PublicAPI]
    public CombatField Field { get; }

    /// <summary>
    /// Calculate the combat state like victory, is progress, ect.
    /// </summary>
    [PublicAPI]
    public ICombatStateStrategy StateStrategy { get; }

    /// <summary>
    /// Complete current turn of combat.
    /// Use in the end of combat movement execution.
    /// </summary>
    public void CompleteTurn()
    {
        var context = new CombatantEffectLifetimeDispelContext(this);

        CombatantEndsTurn?.Invoke(this, new CombatantEndsTurnEventArgs(CurrentCombatant));

        CurrentCombatant.UpdateStatuses(CombatantStatusUpdateType.EndCombatantTurn, context);

        if (_roundQueue.Any())
        {
            RemoveCurrentCombatantFromRoundQueue();
        }

        while (true)
        {
            if (!_roundQueue.Any())
            {
                UpdateAllCombatantEffects(CombatantStatusUpdateType.EndRound, context);

                StartRound(context, false);
                
                var combatState = CalculateCurrentCombatState();
                if (CalculateCurrentCombatState().IsFinalState)
                {
                    CombatFinished?.Invoke(this, new CombatFinishedEventArgs(combatState));
                    return;
                }

                CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));

                return;
            }

            if (_roundQueue.First().IsDead)
            {
                RemoveCurrentCombatantFromRoundQueue();
            }
            else
            {
                var combatState = CalculateCurrentCombatState();
                if (combatState.IsFinalState)
                {
                    CombatFinished?.Invoke(this, new CombatFinishedEventArgs(combatState));
                    return;
                }

                break;
            }
        }

        CurrentCombatant.UpdateStatuses(CombatantStatusUpdateType.StartCombatantTurn, context);

        CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));
    }

    /// <summary>
    /// Create combat move execution to visualize and apply effects in the right way.
    /// </summary>
    public abstract CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement);

    public void DispelCombatantEffect(ICombatant targetCombatant, ICombatantStatus combatantEffect)
    {
        targetCombatant.RemoveStatus(combatantEffect, new CombatantEffectLifetimeDispelContext(this));
        CombatantEffectHasBeenDispeled?.Invoke(this, new CombatantEffectEventArgs(targetCombatant, combatantEffect));
    }

    public int HandleCombatantDamagedToStat(ICombatant combatant, ICombatantStatType statType, StatDamage damage)
    {
        var (remains, wasTaken) = TakeStat(combatant, statType, damage.Amount);

        if (damage.SourceAmount > 0)
        {
            CombatantHasBeenDamaged?.Invoke(this, new CombatantDamagedEventArgs(combatant, statType, damage));
        }

        if (DetectCombatantIsDead(combatant))
        {
            var shiftShape = DetectShapeShifting();
            if (shiftShape)
            {
                CombatantShiftShaped?.Invoke(this, new CombatantShiftShapedEventArgs(combatant));
            }

            combatant.SetDead();
            CombatantHasBeenDefeated?.Invoke(this, new CombatantDefeatedEventArgs(combatant));

            var targetSide = GetTargetSide(combatant, Field);
            var coords = targetSide.GetCombatantCoords(combatant);
            targetSide[coords].Combatant = null;
        }

        return remains;
    }

    public void ImposeCombatantEffect(ICombatant targetCombatant, ICombatantStatus combatantEffect)
    {
        targetCombatant.AddStatus(combatantEffect, new CombatantEffectImposeContext(this),
            new CombatantEffectLifetimeImposeContext(targetCombatant, this));
        CombatantEffectHasBeenImposed?.Invoke(this, new CombatantEffectEventArgs(targetCombatant, combatantEffect));
    }

    /// <summary>
    /// Initialize combat.
    /// </summary>
    /// <param name="heroes">Combatants of hero side (player)</param>
    /// <param name="monsters">Combatants of monster side (CPU).</param>
    [PublicAPI]
    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, Field.HeroSide);
        InitializeCombatFieldSide(monsters, Field.MonsterSide);

        foreach (var combatant in AllCombatantList)
        {
            var startUpContext = new CombatantStartupContext(new CombatantEffectImposeContext(this),
                new CombatantEffectLifetimeImposeContext(combatant, this));
            combatant.PrepareToCombat(startUpContext);
        }

        var context = new CombatantEffectLifetimeDispelContext(this);
        StartRound(context, true);

        CombatRoundStarted?.Invoke(this, EventArgs.Empty);
        CombatantStartsTurn?.Invoke(this, new CombatantTurnStartedEventArgs(CurrentCombatant));
    }

    /// <summary>
    /// Use fo stun.
    /// </summary>
    public void Interrupt()
    {
        CombatantInterrupted?.Invoke(this, new CombatantInterruptedEventArgs(CurrentCombatant));
        CompleteTurn();
    }

    /// <summary>
    /// Maneuvering of combatant.
    /// </summary>
    /// <param name="combatStepDirection"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void UseManeuver(CombatStepDirection combatStepDirection)
    {
        var currentCoords = GetCurrentCoords();

        var targetCoords = GetCoordsByDirection(combatStepDirection, currentCoords);

        var side = GetCurrentSelectorContext().ActorSide;

        HandleSwapFieldPositions(currentCoords, side, targetCoords, side);

        SpendManeuverResources();
    }

    /// <summary>
    /// Used by combatants to restore Resolve stat.
    /// </summary>
    [PublicAPI]
    public void Wait()
    {
        RestoreStatsOnWait();

        CompleteTurn();
    }

    protected abstract bool DetectCombatantIsDead(ICombatant combatant);

    [PublicAPI]
    protected void DoCombatantUsedMovement(ICombatant combatant, CombatMovementInstance movement, int handSlotIndex)
    {
        CombatantUsedMove?.Invoke(this,
            new CombatantHandChangedEventArgs(combatant, movement, handSlotIndex));
    }

    [PublicAPI]
    protected void DoCombatMovementAddToContainer(ICombatant combatant, CombatMovementInstance nextMove,
        int handSlotIndex)
    {
        CombatantAssignedNewMove?.Invoke(this,
            new CombatantHandChangedEventArgs(combatant, nextMove, handSlotIndex));
    }

    [PublicAPI]
    protected ITargetSelectorContext GetCurrentSelectorContext()
    {
        return GetSelectorContext(CurrentCombatant, null);
    }

    /// <summary>
    /// Get context of the target selector.
    /// </summary>
    /// <param name="actor"> Actor of the combat movement. </param>
    /// <param name="attacker"> Current attacker. </param>
    /// <returns> Returns instance of the target selector context. </returns>
    /// <remarks>
    /// In the attack auto-reaction the actor and the attacker is different combatants.
    /// </remarks>
    [PublicAPI]
    protected ITargetSelectorContext GetSelectorContext(ICombatant actor, ICombatant? attacker)
    {
        if (actor.IsPlayerControlled)
        {
            return new TargetSelectorContext(Field.HeroSide, Field.MonsterSide, Dice, attacker);
        }

        return new TargetSelectorContext(Field.MonsterSide, Field.HeroSide, Dice, attacker);
    }

    protected void HandleSwapFieldPositions(FieldCoords sourceCoords, CombatFieldSide sourceFieldSide,
        FieldCoords destinationCoords, CombatFieldSide destinationFieldSide)
    {
        if (sourceCoords == destinationCoords && sourceFieldSide == destinationFieldSide)
        {
            return;
        }

        var sourceCombatant = sourceFieldSide[sourceCoords].Combatant;
        var targetCombatant = destinationFieldSide[destinationCoords].Combatant;

        destinationFieldSide[destinationCoords].Combatant = sourceCombatant;

        if (sourceCombatant is not null)
        {
            CombatantHasChangePosition?.Invoke(this,
                new CombatantHasChangedPositionEventArgs(sourceCombatant, destinationFieldSide, destinationCoords));
        }

        sourceFieldSide[sourceCoords].Combatant = targetCombatant;

        if (targetCombatant is not null)
        {
            CombatantHasChangePosition?.Invoke(this,
                new CombatantHasChangedPositionEventArgs(targetCombatant, sourceFieldSide, sourceCoords));
        }
    }

    protected abstract void PrepareCombatantsToNextRound();

    protected void RemoveCombatantFromQueue(ICombatant combatant)
    {
        _roundQueue.Remove(combatant);
    }

    protected abstract void RestoreStatsOnWait();

    protected abstract void SpendManeuverResources();

    private ICombatState CalculateCurrentCombatState()
    {
        var context = new CombatStateStrategyContext(CurrentCombatants, CurrentRoundNumber);
        return StateStrategy.CalculateCurrentState(context);
    }

    private bool DetectShapeShifting()
    {
        return false;
    }

    private void DoCombatantHasBeenAdded(CombatFieldSide targetSide, FieldCoords targetCoords, ICombatant combatant)
    {
        var combatFieldInfo = new CombatFieldInfo(targetSide, targetCoords);
        var args = new CombatantHasBeenAddedEventArgs(combatant, combatFieldInfo);
        CombatantHasBeenAdded?.Invoke(this, args);
    }

    private static FieldCoords GetCoordsByDirection(CombatStepDirection combatStepDirection, FieldCoords currentCoords)
    {
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
        return targetCoords;
    }

    private FieldCoords GetCurrentCoords()
    {
        var side = GetCurrentSelectorContext().ActorSide;

        for (var col = 0; col < side.ColumnCount; col++)
        {
            for (var lineIndex = 0; lineIndex < side.LineCount; lineIndex++)
            {
                if (CurrentCombatant == side[new FieldCoords(col, lineIndex)].Combatant)
                {
                    return new FieldCoords(col, lineIndex);
                }
            }
        }

        throw new InvalidOperationException();
    }

    private static CombatFieldSide GetTargetSide(ICombatant target, CombatField field)
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

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> formationSlots, CombatFieldSide side)
    {
        foreach (var slot in formationSlots)
        {
            if (slot.Combatant is null)
            {
                continue;
            }

            var coords = new FieldCoords(slot.ColumnIndex, slot.LineIndex);
            side[coords].Combatant = slot.Combatant;

            AllCombatantList.Add(slot.Combatant);

            DoCombatantHasBeenAdded(targetSide: side, targetCoords: coords, slot.Combatant);
        }
    }

    private void MakeCombatantRoundQueue()
    {
        _roundQueue.Clear();

        var combatantQueue = _roundQueueResolver.GetCurrentRoundQueue(AllCombatantList.ToArray());

        foreach (var unit in combatantQueue)
        {
            _roundQueue.Add(unit);
        }
    }

    private void RemoveCurrentCombatantFromRoundQueue()
    {
        _roundQueue.RemoveAt(0);
    }

    private void StartRound(ICombatantStatusLifetimeDispelContext combatantEffectLifetimeDispelContext, bool isFirstRound)
    {
        MakeCombatantRoundQueue();
        PrepareCombatantsToNextRound();

        UpdateAllCombatantEffects(CombatantStatusUpdateType.StartRound, combatantEffectLifetimeDispelContext);
        CurrentCombatant.UpdateStatuses(CombatantStatusUpdateType.StartCombatantTurn,
            combatantEffectLifetimeDispelContext);

        if (!isFirstRound)
        {
            CurrentRoundNumber++;
        }

        CombatRoundStarted?.Invoke(this, EventArgs.Empty);
    }

    private static (int result, bool isTaken) TakeStat(ICombatant combatant, ICombatantStatType statType, int value)
    {
        var stat = combatant.Stats.SingleOrDefault(x => x.Type == statType);

        if (stat is null)
        {
            return (value, false);
        }

        var d = Math.Min(value, stat.Value.Current);
        stat.Value.Consume(d);

        var remains = value - d;

        return (remains, d > 0);
    }

    private void UpdateAllCombatantEffects(CombatantStatusUpdateType updateType,
        ICombatantStatusLifetimeDispelContext context)
    {
        foreach (var combatant in AllCombatantList)
        {
            if (!combatant.IsDead)
            {
                combatant.UpdateStatuses(updateType, context);
            }
        }
    }

    [PublicAPI]
    public event EventHandler<CombatantHasBeenAddedEventArgs>? CombatantHasBeenAdded;

    [PublicAPI]
    public event EventHandler<CombatantTurnStartedEventArgs>? CombatantStartsTurn;

    [PublicAPI]
    public event EventHandler<CombatantEndsTurnEventArgs>? CombatantEndsTurn;

    [PublicAPI]
    public event EventHandler<CombatantDamagedEventArgs>? CombatantHasBeenDamaged;

    [PublicAPI]
    public event EventHandler<CombatantDefeatedEventArgs>? CombatantHasBeenDefeated;

    [PublicAPI]
    public event EventHandler<CombatantShiftShapedEventArgs>? CombatantShiftShaped;

    [PublicAPI]
    public event EventHandler<CombatantHasChangedPositionEventArgs>? CombatantHasChangePosition;

    [PublicAPI]
    public event EventHandler<CombatFinishedEventArgs>? CombatFinished;

    [PublicAPI]
    public event EventHandler<CombatantInterruptedEventArgs>? CombatantInterrupted;

    [PublicAPI]
    public event EventHandler<CombatantHandChangedEventArgs>? CombatantAssignedNewMove;

    [PublicAPI]
    public event EventHandler<CombatantHandChangedEventArgs>? CombatantUsedMove;

    [PublicAPI]
    public event EventHandler<CombatantEffectEventArgs>? CombatantEffectHasBeenImposed;

    [PublicAPI]
    public event EventHandler<CombatantEffectEventArgs>? CombatantEffectHasBeenDispeled;

    public event EventHandler? CombatRoundStarted;

    public sealed record StatDamage(int Amount, int SourceAmount)
    {
        public static implicit operator StatDamage(int monoValue) { return new StatDamage(monoValue, monoValue); }
    }
}