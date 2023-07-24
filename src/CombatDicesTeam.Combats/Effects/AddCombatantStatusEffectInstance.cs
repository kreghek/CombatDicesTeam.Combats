using CombatDicesTeam.Combats.CombatantStatuses;

namespace CombatDicesTeam.Combats.Effects;

public sealed class AddCombatantStatusEffectInstance : EffectInstanceBase<AddCombatantStatusEffect>
{
    private readonly ICombatantStatusFactory _combatantStatusFactory;

    public AddCombatantStatusEffectInstance(AddCombatantStatusEffect baseEffect,
        ICombatantStatusFactory combatantStatusFactory) : base(baseEffect)
    {
        _combatantStatusFactory = combatantStatusFactory;
    }

    public override void Influence(ICombatant target, IStatusCombatContext context)
    {
        context.StatusImposedContext.ImposeCombatantStatus(target, _combatantStatusFactory);
    }
}