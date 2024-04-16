namespace CombatDicesTeam.Combats;

public interface IEffectInstance
{
    /// <summary>
    /// Conditions for the effect. The effect will be applied to the target only if all conditions are satisfied.
    /// </summary>
    IReadOnlyCollection<IEffectCondition> ImposeConditions { get; }

    /// <summary>
    /// Effect target selector.
    /// </summary>
    ITargetSelector Selector { get; }

    /// <summary>
    /// Add effect modifier.
    /// </summary>
    /// <param name="modifier">Effect stat modifier to add.</param>
    void AddModifier(IStatModifier modifier);

    /// <summary>
    /// Influence effect to target.
    /// </summary>
    /// <param name="target">The target from the selector.</param>
    /// <param name="context"> Context to interact with combat. </param>
    void Influence(ICombatant target, ICombatMovementContext context);

    /// <summary>
    /// Remove effect modifier.
    /// </summary>
    /// <param name="modifier">Effect stat modifier to remove.</param>
    void RemoveModifier(IStatModifier modifier);
}