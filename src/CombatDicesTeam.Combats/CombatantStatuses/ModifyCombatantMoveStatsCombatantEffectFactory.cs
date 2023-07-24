namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class ModifyCombatantMoveStatsCombatantStatusFactory : ICombatantStatusFactory
{
    private readonly ICombatantStatusLifetimeFactory _lifetimeFactory;
    private readonly ICombatantStatusSid _sid;
    private readonly CombatantMoveStats _stats;
    private readonly int _value;

    public ModifyCombatantMoveStatsCombatantStatusFactory(ICombatantStatusSid sid,
        ICombatantStatusLifetimeFactory lifetimeFactory,
        CombatantMoveStats stats, int value)
    {
        _sid = sid;
        _lifetimeFactory = lifetimeFactory;
        _stats = stats;
        _value = value;
    }

    public ICombatantStatus Create()
    {
        return new ModifyCombatantMoveStatsCombatantStatus(_sid, _lifetimeFactory.Create(), _stats, _value);
    }
}