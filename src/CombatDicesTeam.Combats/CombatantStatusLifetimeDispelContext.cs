namespace CombatDicesTeam.Combats;

// ReSharper disable once InconsistentNaming
public sealed class CombatantStatusLifetimeDispelContext : ICombatantStatusLifetimeDispelContext
{
    public CombatantStatusLifetimeDispelContext(CombatEngineBase combat)
    {
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }
}