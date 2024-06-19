namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class CounterCombatantStatus : CombatantStatusBase
{
    private readonly ICounterReaction _counterReaction;
    private CombatEngineBase? _combat;
    private ICombatant? _ownerCombatant;

    public CounterCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
        ICombatantStatusSource source, ICounterReaction counterReaction) : base(sid, lifetime, source)
    {
        _counterReaction = counterReaction;
    }

    public override void Dispel(ICombatant combatant)
    {
        _combat!.CombatantStatChanged -= Combat_CombatantHasBeenDamaged;
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        _combat = context.Combat;
        _ownerCombatant = combatant;

        _combat.CombatantStatChanged += Combat_CombatantHasBeenDamaged;
    }

    private void Combat_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        if (_ownerCombatant == e.Combatant)
        {
            _counterReaction.Occur(this, _ownerCombatant, _combat!);
        }
    }
}