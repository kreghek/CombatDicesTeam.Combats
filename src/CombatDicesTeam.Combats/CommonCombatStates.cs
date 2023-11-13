namespace CombatDicesTeam.Combats;

/// <summary>
/// List of common combat states.
/// </summary>
public static class CommonCombatStates
{
    public static ICombatState InProgress { get; } = new CombatState(false);
    public static ICombatState Victory { get; } = new CombatState();
    public static ICombatState Failure { get; } = new CombatState();
    public static ICombatState Draw { get; } = new CombatState();
}