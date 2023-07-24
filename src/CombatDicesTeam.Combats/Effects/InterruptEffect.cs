namespace CombatDicesTeam.Combats.Effects;

public class InterruptEffect : IEffect
{
    public InterruptEffect(ITargetSelector selector)
    {
        Selector = selector;
    }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new InterruptEffectInstance(this);
    }
}