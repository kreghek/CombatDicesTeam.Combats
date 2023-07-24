namespace Core.Combats;

/// <summary>
/// Combatant's status lifetime to control the status must be dispelled.
/// </summary>
public interface ICombatantStatusLifetime
{
    /// <summary>
    /// Status is expired and must be dispelled.
    /// </summary>
    bool IsExpired { get; }

    /// <summary>
    /// Do work to release lifetime object then status-owner was dispelled.
    /// </summary>
    /// <param name="owner">Status owns lifetime object.</param>
    /// <param name="context"> Context object to access to combat environment. </param>
    /// <remarks>
    /// Example:
    /// Used to monitor combatant activities.
    /// </remarks>
    void HandleDispelling(ICombatantStatus owner, ICombatantStatusLifetimeDispelContext context);

    /// <summary>
    /// Do work to prepare lifetime object then status-owner was imposed.
    /// </summary>
    /// <param name="owner"> Status owns lifetime object. </param>
    /// <param name="context"> Context object to access to combat environment. </param>
    /// <remarks>
    /// Example:
    /// Used to monitor combatant activities.
    /// </remarks>
    void HandleImposed(ICombatantStatus owner, ICombatantStatusLifetimeImposeContext context);

    /// <summary>
    /// Updates status state according combat life cycle.
    /// </summary>
    /// <param name="updateType"> Current combat life cycle. </param>
    /// <param name="context"> Context object to access to combat environment. </param>
    void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context);
}