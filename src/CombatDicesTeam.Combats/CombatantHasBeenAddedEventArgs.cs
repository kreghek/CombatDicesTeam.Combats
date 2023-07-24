namespace CombatDicesTeam.Combats;

public sealed class CombatantHasBeenAddedEventArgs : CombatantEventArgsBase
{
    public CombatantHasBeenAddedEventArgs(ICombatant combatant, CombatFieldInfo fieldInfo) : base(combatant)
    {
        FieldInfo = fieldInfo;
    }

    public CombatFieldInfo FieldInfo { get; }
}