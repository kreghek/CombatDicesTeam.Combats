namespace CombatDicesTeam.Combats.CombatantStatuses;

/// <summary>
/// Common factory to create status with any delegate.
/// </summary>
public sealed class CombatStatusFactory : ICombatantStatusFactory
{
    private readonly CombatStatusFactoryDelegate _createDelegate;

    public CombatStatusFactory(CombatStatusFactoryDelegate createDelegate)
    {
        _createDelegate = createDelegate;
    }

    /// <inheritdoc />
    public ICombatantStatus Create(ICombatantStatusSource source)
    {
        return _createDelegate(source);
    }
}