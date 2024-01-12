using System.Diagnostics;

namespace CombatDicesTeam.Combats;

[DebuggerDisplay("{DebugSid}")]
public sealed record PositionChangeReason(string? DebugSid = null) : IPositionChangingReason;
