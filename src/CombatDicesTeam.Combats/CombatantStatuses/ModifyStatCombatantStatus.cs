namespace CombatDicesTeam.Combats.CombatantStatuses;

public delegate int StatusValueCalculatorDelegate(ICombatantStatus status);

/// <summary>
/// Change max value of specified combatant's stat.
/// </summary>
public sealed class ModifyStatCombatantStatus : CombatantStatusBase
{
    private readonly IStatModifier _statModifier;
    private readonly StatusValueCalculatorDelegate _valueCalculator;

    public ModifyStatCombatantStatus(ICombatantStatusSid sid,
        ICombatantStatusLifetime lifetime,
        ICombatantStatusSource source,
        ICombatantStatType statType,
        int value) :
        this(sid, lifetime, source, statType, _ => value)
    {
    }

    public ModifyStatCombatantStatus(ICombatantStatusSid sid,
        ICombatantStatusLifetime lifetime,
        ICombatantStatusSource source,
        ICombatantStatType statType,
        StatusValueCalculatorDelegate valueCalculator) :
        base(sid, lifetime, source)
    {
        _valueCalculator = valueCalculator;
        StatType = statType;

        _statModifier = new StatModifier(() => _valueCalculator(this), new CombatantStatusModifierSource(this));
    }

    public ICombatantStatType StatType { get; }
    public int Value => _valueCalculator(this);

    public override void Dispel(ICombatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.RemoveModifier(_statModifier);
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.AddModifier(_statModifier);
    }
}