using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

/// <summary>
/// Base class of event args of combatant.
/// </summary>
[PublicAPI]
public abstract class CombatantEventArgsBase : EventArgs
{
    protected CombatantEventArgsBase(ICombatant combatant)
    {
        Combatant = combatant;
    }

    public ICombatant Combatant { get; }
}