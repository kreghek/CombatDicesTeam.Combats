using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

/// <summary>
/// Event arguments to pass combatant status in a status' events.
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class CombatantStatusEventArgs : CombatantEventArgsBase
{
    public CombatantStatusEventArgs(ICombatant combatant, ICombatantStatus status) :
        base(combatant)
    {
        Status = status;
    }

    [PublicAPI]
    public ICombatantStatus Status { get; }
}