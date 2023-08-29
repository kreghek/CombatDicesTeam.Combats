namespace CombatDicesTeam.Combats;

public interface ICombatActorBehaviourDataProvider
{
    ICombatantBehaviourData GetDataSnapshot();
}