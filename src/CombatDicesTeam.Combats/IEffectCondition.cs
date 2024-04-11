namespace CombatDicesTeam.Combats;

public interface IEffectCondition
{
    /// <summary>
    /// Checks the condition.
    /// </summary>
    /// <param name="actor">A combatant which imposing effect.</param>
    /// <param name="combatField">A combat battlefield.</param>
    /// <returns>True is the condition is satisfied. Overwise, false.</returns>
    bool Check(ICombatant actor, CombatField combatField);
}