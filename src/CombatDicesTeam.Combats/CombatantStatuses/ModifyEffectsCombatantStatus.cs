namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// Change combat movements effects. Damage, etc.
/// </summary>
public sealed class ModifyEffectsCombatantStatus : CombatantStatusBase
{
    private readonly IUnitStatModifier _statModifier;

    public ModifyEffectsCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime, int value) : base(sid, lifetime)
    {
        Value = value;
        _statModifier = new StatModifier(Value);
    }

    public int Value { get; }

    public override void Dispel(ICombatant combatant)
    {
        base.Dispel(combatant);

        foreach (var combatMovementContainer in combatant.CombatMovementContainers)
        {
            foreach (var combatMovementInstance in combatMovementContainer.GetItems())
            {
                if (combatMovementInstance is not null)
                {
                    foreach (var effectInstance in combatMovementInstance.Effects)
                    {
                        effectInstance.RemoveModifier(_statModifier);
                    }
                }
            }
        }

    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext context)
    {
        base.Impose(combatant, context);

        foreach (var combatMovementContainer in combatant.CombatMovementContainers)
        {
            foreach (var combatMovementInstance in combatMovementContainer.GetItems())
            {
                if (combatMovementInstance is not null)
                {
                    foreach (var effectInstance in combatMovementInstance.Effects)
                    {
                        effectInstance.AddModifier(_statModifier);
                    }
                }
            }
        }
    }
}