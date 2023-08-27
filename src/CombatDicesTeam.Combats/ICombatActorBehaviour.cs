namespace CombatDicesTeam.Combats;

public interface ICombatActorBehaviour
{
    void HandleIntention(ICombatantBehaviourData combatData, Action<IIntention> intentionDelegate);
}