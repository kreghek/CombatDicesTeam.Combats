namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class AuraCombatantStatus : CombatantStatusBase
{
    private readonly Func<ICombatant, ICombatantStatusFactory> _auraStatusDelegate;
    private readonly IAuraTargetSelector _auraTargetSelector;
    private CombatEngineBase? _combat;

    private ICombatant? _owner;

    public AuraCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
        ICombatantStatusSource source, Func<ICombatant, ICombatantStatusFactory> auraStatusDelegate,
        IAuraTargetSelector auraTargetSelector) :
        base(sid, lifetime, source)
    {
        _auraStatusDelegate = auraStatusDelegate;
        _auraTargetSelector = auraTargetSelector;
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        base.Impose(combatant, context);

        _owner = combatant;
        _combat = context.Combat;

        var auraContext = new AuraTargetSelectorContext(_combat);

        var auraTargets = context.Combat.CurrentCombatants
            .Where(x => _auraTargetSelector.IsCombatantUnderAura(_owner, x, auraContext));

        // Add status to current combatants
        foreach (var target in auraTargets)
        {
            AddAuraStatus(target, combatant, context.Combat);
        }

        context.Combat.CombatantHasBeenAdded += Combat_CombatantHasBeenAdded;
    }

    private void AddAuraStatus(ICombatant auraStatusTarget, ICombatant auraOwner, CombatEngineBase combat)
    {
        auraStatusTarget.AddStatus(_auraStatusDelegate(auraOwner).Create(Source),
            new CombatantStatusImposeContext(combat),
            new CombatantStatusLifetimeImposeContext(auraOwner, combat));
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

            var auraContext = new AuraTargetSelectorContext(_combat);

            if (!_auraTargetSelector.IsCombatantUnderAura(_owner, combatant, auraContext))
            {
                // Do not add status new allies
                return;
            }

            // Add status to new combatant

            AddAuraStatus(combatant, combatant, _combat);
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