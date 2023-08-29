namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// Change max value of specified combatant's stat.
/// </summary>
public sealed class ChangeStatCombatantStatus : CombatantStatusBase
{
    private readonly IStatModifier _statModifier;

    public ChangeStatCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime,
        ICombatantStatType statType,
        int value) :
        base(sid, lifetime)
    {
        StatType = statType;
        Value = value;

        _statModifier = new StatModifier(value, Singleton<NullStatModifierSource>.Instance);
    }

    public ICombatantStatType StatType { get; }
    public int Value { get; }

    public override void Dispel(ICombatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.RemoveModifier(_statModifier);
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.AddModifier(_statModifier);
    }
}