namespace CombatDicesTeam.Combats;

public sealed class CombatantEffectLifetimeDispelContext : ICombatantEffectLifetimeDispelContext
{
    public CombatantEffectLifetimeDispelContext(CombatEngine combat)
    {
        Combat = combat;
    }

    public CombatEngine Combat { get; }
}