namespace CombatDicesTeam.Combats;

public static class CommonPositionChangeReasons
{
    public static IPositionChangingReason Maneuver { get; } = new PositionChangeReason(nameof(Maneuver));
}
