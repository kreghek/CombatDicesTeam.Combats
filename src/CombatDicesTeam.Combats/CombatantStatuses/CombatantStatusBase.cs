namespace Core.Combats.CombatantStatuses;

public abstract class CombatantStatusBase : ICombatantStatus
{
    protected CombatantStatusBase(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime)
    {
        Sid = sid;
        Lifetime = lifetime;
    }

    public ICombatantStatusSid Sid { get; }
    public ICombatantStatusLifetime Lifetime { get; }

    public virtual void Dispel(ICombatant combatant)
    {
    }

    public virtual void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
    }

    public virtual void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        Lifetime.Update(updateType, context);
    }
}