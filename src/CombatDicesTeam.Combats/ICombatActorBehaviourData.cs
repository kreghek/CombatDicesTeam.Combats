namespace CombatDicesTeam.Combats;

public interface ICombatActorBehaviourData
{
    IReadOnlyCollection<CombatUnitBehaviourDataActor> Actors { get; }
    CombatUnitBehaviourDataActor CurrentActor { get; }
}