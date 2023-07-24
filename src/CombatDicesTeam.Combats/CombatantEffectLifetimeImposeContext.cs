namespace Core.Combats;

public sealed class CombatantEffectLifetimeImposeContext : ICombatantStatusLifetimeImposeContext
{
    public CombatantEffectLifetimeImposeContext(ICombatant targetCombatant, CombatEngineBase combat)
    {
        TargetCombatant = targetCombatant;
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }
    public ICombatant TargetCombatant { get; }
}