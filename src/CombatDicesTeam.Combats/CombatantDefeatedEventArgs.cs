using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed class CombatantDefeatedEventArgs : CombatantEventArgsBase
{
    public CombatantDefeatedEventArgs(ICombatant combatant) : base(combatant)
    {
    }
}