namespace CombatDicesTeam.Combats;

public interface IEffect
{
    /// <summary>
    /// Conditions for the effect. The effect will be applied to the target only if all conditions are satisfied.
    /// </summary>
    IReadOnlyCollection<IEffectCondition> ImposeConditions { get; }

    ITargetSelector Selector { get; }

    IEffectInstance CreateInstance();
}