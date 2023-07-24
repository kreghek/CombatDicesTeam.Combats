namespace CombatDicesTeam.Combats.Effects;

public sealed class AdjustPositionEffect : IEffect
{
    public AdjustPositionEffect(ITargetSelector selector)
    {
        Selector = selector;
    }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new AdjustPositionEffectInstance(this);
    }
}