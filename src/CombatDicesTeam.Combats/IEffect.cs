namespace Core.Combats;

public interface IEffect
{
    IReadOnlyCollection<IEffectCondition> ImposeConditions { get; }
    ITargetSelector Selector { get; }

    IEffectInstance CreateInstance();
}