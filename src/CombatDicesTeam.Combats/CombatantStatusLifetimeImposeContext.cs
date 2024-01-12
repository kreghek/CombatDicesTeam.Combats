namespace CombatDicesTeam.Combats;

public sealed class CombatantStatusLifetimeImposeContext : ICombatantStatusLifetimeImposeContext
{
    public CombatantStatusLifetimeImposeContext(ICombatant targetCombatant, CombatEngineBase combat)
    {
        TargetCombatant = targetCombatant;
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }
    public ICombatant TargetCombatant { get; }
}