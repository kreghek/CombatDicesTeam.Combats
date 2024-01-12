namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// Combatant with tis condition will pass next turn.
/// </summary>
public class StunCombatantStatus : CombatantStatusBase
{
    public StunCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime, ICombatantStatusSource source) : base(sid, lifetime, source)
    {
    }

    public override void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        if (updateType == CombatantStatusUpdateType.StartCombatantTurn)
        {
            context.CompleteTurn();
        }
    }
}