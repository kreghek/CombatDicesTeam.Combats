namespace CombatDicesTeam.Combats.CombatantEffectLifetimes;

public sealed class MultipleCombatantTurnEffectLifetime : ICombatantStatusLifetime
{
    private bool _currentRoundEnd;

    public MultipleCombatantTurnEffectLifetime(int duration)
    {
        Counter = duration;
    }

    public int Counter { get; set; }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        if (updateType == CombatantStatusUpdateType.EndRound)
        {
            _currentRoundEnd = true;
        }

        if (_currentRoundEnd && updateType == CombatantStatusUpdateType.EndRound)
        {
            Counter--;
            if (Counter == 0)
            {
                IsExpired = true;
            }
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