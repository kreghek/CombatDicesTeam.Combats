namespace Core.Combats;

/// <summary>
/// Status assigned on a combatant.
/// </summary>
public interface ICombatantStatus
{
    ICombatantStatusLifetime Lifetime { get; }
    ICombatantStatusSid Sid { get; }
    void Dispel(ICombatant combatant);
    void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext);
    void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context);
}