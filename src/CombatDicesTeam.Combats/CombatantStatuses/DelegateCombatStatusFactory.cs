namespace CombatDicesTeam.Combats.CombatantStatuses;

public sealed class DelegateCombatStatusFactory : ICombatantStatusFactory
{
    private readonly Func<ICombatantStatusSource, ICombatantStatus> _createDelegate;

    public DelegateCombatStatusFactory(Func<ICombatantStatusSource, ICombatantStatus> createDelegate)
    {
        _createDelegate = createDelegate;
    }

    public ICombatantStatus Create(ICombatantStatusSource source)
    {
        return _createDelegate(source);
    }
}