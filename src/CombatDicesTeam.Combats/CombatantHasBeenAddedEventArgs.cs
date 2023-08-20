using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

[PublicAPI]
public sealed class CombatantHasBeenAddedEventArgs : CombatantEventArgsBase
{
    public CombatantHasBeenAddedEventArgs(ICombatant combatant, CombatFieldInfo fieldInfo) : base(combatant)
    {
        FieldInfo = fieldInfo;
    }

    [PublicAPI]
    public CombatFieldInfo FieldInfo { get; }
}