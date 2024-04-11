using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats.Effects;

public sealed class AddCombatantStatusEffect : IEffect
{
    private readonly ICombatantStatusFactory _combatantEffectFactory;


    public AddCombatantStatusEffect(ITargetSelector targetSelector, ICombatantStatusFactory combatantEffectFactory)
    {
        _combatantEffectFactory = combatantEffectFactory;

        Selector = targetSelector;
    }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions { get; init; } = Array.Empty<IEffectCondition>();
    public ITargetSelector Selector { get; }

    public IEffectInstance CreateInstance()
    {
        return new AddCombatantStatusEffectInstance(this, _combatantEffectFactory);
    }
}