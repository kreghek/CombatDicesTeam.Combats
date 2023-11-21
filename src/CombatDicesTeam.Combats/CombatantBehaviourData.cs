using JetBrains.Annotations;

namespace CombatDicesTeam.Combats;

/// <summary>
/// Snapshot of combat situation to use it in behaviour of combatants.
/// </summary>
/// <param name="CombatMoves">Available moves of behaviour's combatant.</param>
[PublicAPI]
public record CombatantBehaviourData(IReadOnlyCollection<CombatantMoveBehaviourData> CombatMoves);