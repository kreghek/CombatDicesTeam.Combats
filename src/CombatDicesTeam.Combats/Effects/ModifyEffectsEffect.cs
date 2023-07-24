namespace CombatDicesTeam.Combats.Effects;

public sealed class ModifyEffectsEffect : IEffect
{
    private readonly ICombatantStatusSid _statusSid;

    public ModifyEffectsEffect(ICombatantStatusSid statusSid, ITargetSelector selector, int value)
    {
        _statusSid = statusSid;
        Selector = selector;
        Value = value;
    }

    public int Value { get; }

    public ITargetSelector Selector { get; }

    public IReadOnlyCollection<IEffectCondition> ImposeConditions => Array.Empty<IEffectCondition>();

    public IEffectInstance CreateInstance()
    {
        return new ModifyEffectsEffectInstance(_statusSid, this);
    }
}