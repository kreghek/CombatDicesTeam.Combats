using CombatDicesTeam.Combats.Effects;

namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class AddStatusCounterReaction : ICounterReaction
{
    private readonly ICombatantStatusFactory _statusFactory;
    private readonly IAuraTargetSelector _statusSelector;

    public AddStatusCounterReaction(ICombatantStatusFactory statusFactory, IAuraTargetSelector statusSelector)
    {
        _statusFactory = statusFactory;
        _statusSelector = statusSelector;
    }

    public void Occur(CounterCombatantStatus counterCombatantStatus, ICombatant ownerCombatant, CombatEngineBase combat)
    {
        var combatants = combat.CurrentCombatants;

        foreach (var combatant in combatants)
        {
            if (!_statusSelector.IsCombatantUnderAura(ownerCombatant, combatant, new AuraTargetSelectorContext(combat)))
            {
                continue;
            }

            var status = _statusFactory.Create(new CounterCombatantStatusSource(counterCombatantStatus));

            combat.ImposeCombatantStatus(combatant, status);
        }
    }
}