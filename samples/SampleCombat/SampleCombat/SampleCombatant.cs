using CombatDicesTeam.Combats;

namespace SampleCombat;

public class SampleCombatant: ICombatant
{
    private readonly CombatMovementContainer _combatMoveContainer;
    
    private readonly IList<ICombatantStatus> _statuses ;
    
    public ICombatActorBehaviour Behaviour { get; }
    public string ClassSid { get; }
    public IReadOnlyCollection<ICombatMovementContainer> CombatMovementContainers => new[] { _combatMoveContainer };
    public string? DebugSid { get; init; }
    public bool IsDead { get; private set; }
    public bool IsPlayerControlled { get; init; }
    public IReadOnlyCollection<ICombatantStat> Stats { get; }
    public IReadOnlyCollection<ICombatantStatus> Statuses => _statuses.ToArray();

    public SampleCombatant(string classSid, IEnumerable<CombatMovementInstance> combatMoves, ICombatActorBehaviour behaviour)
    {
        ClassSid = classSid;
        Behaviour = behaviour;

        _combatMoveContainer = new CombatMovementContainer(new SampleCombatMovementContainerType());
        _statuses = new List<ICombatantStatus>();

        Stats = new[] { new CombatantStat(SampleCombatantStatTypes.HP, new StatValue(10)) };
        
        foreach (var combatMove in combatMoves)
        {
            _combatMoveContainer.AppendMove(combatMove);
        }
    }
    
    public void AddStatus(ICombatantStatus effect, ICombatantStatusImposeContext statusImposeContext,
        ICombatantStatusLifetimeImposeContext lifetimeImposeContext)
    {
        effect.Impose(this, statusImposeContext);
        _statuses.Add(effect);

        effect.Lifetime.HandleImposed(effect, lifetimeImposeContext);
    }

    public void PrepareToCombat(ICombatantStartupContext context)
    {
        
    }

    public void RemoveStatus(ICombatantStatus effect, ICombatantStatusLifetimeDispelContext context)
    {
        effect.Dispel(this);
        _statuses.Remove(effect);

        effect.Lifetime.HandleDispelling(effect, context);
    }

    public void SetDead()
    {
        IsDead = true;
    }

    public void UpdateStatuses(CombatantStatusUpdateType updateType,
        ICombatantStatusLifetimeDispelContext effectLifetimeDispelContext)
    {
        var context = new CombatantStatusLifetimeUpdateContext(this, effectLifetimeDispelContext.Combat);

        var statusesToDispel = new List<ICombatantStatus>();
        foreach (var status in _statuses)
        {
            status.Update(updateType, context);

            if (status.Lifetime.IsExpired)
            {
                statusesToDispel.Add(status);
            }
        }

        foreach (var effect in statusesToDispel)
        {
            effect.Dispel(this);
            RemoveStatus(effect, effectLifetimeDispelContext);
        }
    }
}