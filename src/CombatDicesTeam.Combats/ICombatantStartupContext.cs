namespace CombatDicesTeam.Combats;

public interface ICombatantStartupContext
{
    ICombatantStatusImposeContext ImposeStatusContext { get; }
    ICombatantStatusLifetimeImposeContext ImposeStatusLifetimeContext { get; }
}

public sealed record CombatantStartupContext(ICombatantStatusImposeContext ImposeStatusContext, ICombatantStatusLifetimeImposeContext ImposeStatusLifetimeContext) : ICombatantStartupContext { }