namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class AuraCombatantStatus : CombatantStatusBase
{
    private readonly ICombatantStatusFactory _auraStatus;
    private readonly IAuraTargetSelector _auraTargetSelector;
    
    private ICombatant? _owner;
    private CombatEngineBase? _combat;

    public AuraCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
        ICombatantStatusSource source, ICombatantStatusFactory auraStatus, IAuraTargetSelector auraTargetSelector) : base(sid, lifetime, source)
    {
        _auraStatus = auraStatus;
        _auraTargetSelector = auraTargetSelector;
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        base.Impose(combatant, context);
        
        _owner = combatant;
        _combat = context.Combat;

        var auraTargets = context.Combat.CurrentCombatants.Where(x => _auraTargetSelector.IsCombatantUnderAura(x, combatant));

        // Add status to current combatants
        foreach (var target in auraTargets)
        {
            AddReduceStatus(target, combatant, context.Combat);
        }
        
        context.Combat.CombatantHasBeenAdded += Combat_CombatantHasBeenAdded; 
    }
    
    private void AddReduceStatus(ICombatant enemy, ICombatant combatant, CombatEngineBase combat)
    {
        enemy.AddStatus(_auraStatus.Create(Source), new CombatantStatusImposeContext(combat),
            new CombatantStatusLifetimeImposeContext(combatant, combat));
    }
    
    private void Combat_CombatantHasBeenAdded(object? sender, CombatantHasBeenAddedEventArgs e)
    {
        var combatant = e.Combatant;

        if (_combat is not null)
        {
            if (_owner is null)
            {
#if DEBUG
                throw new InvalidOperationException("Status owner was not assigned in Impose method.");
#else
                return;
#endif
            }

            if (!_auraTargetSelector.IsCombatantUnderAura(_owner, combatant))
            {
                // Do not add status new allies
                return;
            }

            // Add status to new combatant

            AddReduceStatus(combatant, combatant, _combat);
        }
        else
        {
#if DEBUG
            throw new InvalidOperationException("Combat engine was not assigned in Impose method");
#else
            return;
#endif
        }
    }
}