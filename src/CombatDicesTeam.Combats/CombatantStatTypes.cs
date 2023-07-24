namespace Core.Combats;

public static class CombatantStatTypes
{
    public static ICombatantStatType Defense { get; } = new CombatantStatType(nameof(Defense));
    public static ICombatantStatType HitPoints { get; } = new CombatantStatType(nameof(HitPoints));
    public static ICombatantStatType Maneuver { get; } = new CombatantStatType(nameof(Maneuver));
    public static ICombatantStatType Resolve { get; } = new CombatantStatType(nameof(Resolve));
    public static ICombatantStatType ShieldPoints { get; } = new CombatantStatType(nameof(ShieldPoints));
}