namespace Core.Combats;

public sealed class CombatantDefeatedEventArgs : CombatantEventArgsBase
{
    public CombatantDefeatedEventArgs(ICombatant combatant) : base(combatant)
    {
    }
}