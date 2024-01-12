namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class ModifyEffectsCombatantStatusFactory : ICombatantStatusFactory
{
    private readonly ICombatantStatusLifetimeFactory _lifetimeFactory;
    private readonly ICombatantStatusSid _sid;
    private readonly int _value;

    public ModifyEffectsCombatantStatusFactory(ICombatantStatusSid sid, ICombatantStatusLifetimeFactory lifetimeFactory,
        int value)
    {
        _sid = sid;
        _lifetimeFactory = lifetimeFactory;
        _value = value;
    }

    public ICombatantStatus Create(ICombatantStatusSource source)
    {
        return new ModifyEffectsCombatantStatus(_sid, _lifetimeFactory.Create(), source, _value);
    }
}