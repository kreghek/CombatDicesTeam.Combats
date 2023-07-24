namespace Core.Combats;

public interface ICombatActorBehaviourDataProvider
{
    ICombatActorBehaviourData GetDataSnapshot();
}