namespace CombatDicesTeam.Combats.CombatantStatuses;

public interface ICounterReaction
{
    void Occur(CounterCombatantStatus counterCombatantStatus, ICombatant ownerCombatant, CombatEngineBase combat);
}