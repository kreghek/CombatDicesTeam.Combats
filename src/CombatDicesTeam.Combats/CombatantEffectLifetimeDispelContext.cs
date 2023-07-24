namespace CombatDicesTeam.Combats;

public sealed class CombatantEffectLifetimeDispelContext : ICombatantStatusLifetimeDispelContext
{
    public CombatantEffectLifetimeDispelContext(CombatEngineBase combat)
    {
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }
}