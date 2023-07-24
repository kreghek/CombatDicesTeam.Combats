namespace Core.Combats;

public interface ICombatant
{
    /// <summary>
    /// Activity source of the combatant. May be CPU-driven or wait player's intentions.
    /// </summary>
    ICombatActorBehaviour Behaviour { get; }

    /// <summary>
    /// Identifier of class.
    /// Class is the group of combatant.
    /// </summary>
    string ClassSid { get; }

    /// <summary>
    /// Identifier for debug.
    /// </summary>
// ReSharper disable once UnusedAutoPropertyAccessor.Global
    string? DebugSid { get; init; }

    /// <summary>
    /// Is the combatant active?
    /// </summary>
    bool IsDead { get; }

    /// <summary>
    /// Combatant side. Player of CPU.
    /// </summary>
    bool IsPlayerControlled { get; init; }

    /// <summary>
    /// Current combatant stats.
    /// </summary>
    IReadOnlyCollection<IUnitStat> Stats { get; }

    /// <summary>
    /// Current combatant effects.
    /// </summary>
    IReadOnlyCollection<ICombatantStatus> Statuses { get; }

    /// <summary>
    /// Add effect to combatant.
    /// </summary>
    /// <param name="effect">Effect instance.</param>
    /// <param name="lifetimeImposeContext">
    /// Content to add effect. To handle some reaction on new effects (change stats, moves, other
    /// effects).
    /// </param>
    void AddStatus(ICombatantStatus effect, ICombatantStatusImposeContext effectImposeContext,
        ICombatantStatusLifetimeImposeContext lifetimeImposeContext);

    /// <summary>
    /// Initial method to make combatant ready to fight.
    /// </summary>
    void PrepareToCombat(ICombatantStartupContext context);

    void RemoveStatus(ICombatantStatus effect, ICombatantStatusLifetimeDispelContext context);

    /// <summary>
    /// Deactivate combatant.
    /// He is not combatant yet.
    /// </summary>
    void SetDead();

    /// <summary>
    /// Update combatant effects.
    /// </summary>
    void UpdateStatuses(CombatantStatusUpdateType updateType,
        ICombatantStatusLifetimeDispelContext effectLifetimeDispelContext);

    IReadOnlyCollection<ICombatMovementContainer> CombatMovementContainers { get; }
}