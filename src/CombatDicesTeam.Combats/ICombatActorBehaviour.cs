namespace CombatDicesTeam.Combats;

public interface ICombatActorBehaviour
{
    void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate);
}