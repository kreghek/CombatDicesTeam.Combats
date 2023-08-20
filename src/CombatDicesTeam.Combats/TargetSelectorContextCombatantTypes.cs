namespace CombatDicesTeam.Combats;

/// <summary>
/// Default types of combatants in the conext.
/// </summary>
public static class TargetSelectorContextCombatantTypes
{ 
    public static ITargetSelectorContextCombatantType Attacker => new TargetSelectorContextCombatantType(nameof(Attacker));
}