namespace CombatDicesTeam.Combats.Effects;

public sealed class GroupEffect : IEffect
{
    private readonly IEffect[] _concreteEffects;

    public GroupEffect(ITargetSelector selector, params IEffect[] concreteEffects)
    {
        Selector = selector;
        _concreteEffects = concreteEffects;
    }

    public ChangePositionEffectDirection Direction { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new GroupEffectInstance(this, _concreteEffects.Select(x => x.CreateInstance()).ToArray());
    }
}