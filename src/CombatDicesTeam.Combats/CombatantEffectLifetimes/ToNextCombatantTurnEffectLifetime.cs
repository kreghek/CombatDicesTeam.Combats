namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public sealed class ToNextCombatantTurnEffectLifetime : ICombatantStatusLifetime
{
    private bool _currentRoundEnd;

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        if (updateType == CombatantStatusUpdateType.EndRound)
        {
            _currentRoundEnd = true;
        }

        if (_currentRoundEnd && updateType == CombatantStatusUpdateType.StartCombatantTurn)
        {
            IsExpired = true;
        }
    }

    public void HandleImposed(ICombatantStatus combatantEffect, ICombatantStatusLifetimeImposeContext context)
    {
    }

    public void HandleDispelling(ICombatantStatus combatantEffect, ICombatantStatusLifetimeDispelContext context)
    {
    }

    public bool IsExpired { get; private set; }
}