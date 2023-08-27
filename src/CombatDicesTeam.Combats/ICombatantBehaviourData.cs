namespace CombatDicesTeam.Combats;

public interface ICombatantBehaviourData
{
    IReadOnlyCollection<CombatantBehaviourData> Actors { get; }
    CombatantBehaviourData CurrentActor { get; }
}