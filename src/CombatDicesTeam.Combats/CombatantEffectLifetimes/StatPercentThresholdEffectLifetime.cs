namespace Core.Combats.CombatantEffectLifetimes;

internal class StatPercentThresholdEffectLifetime : ICombatantStatusLifetime
{
    private readonly float _minShare;
    private readonly ICombatantStatType _statType;

    public StatPercentThresholdEffectLifetime(ICombatantStatType statType, float minShare)
    {
        _statType = statType;
        _minShare = minShare;
    }

    public bool IsExpired { get; private set; }

    public void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        if (updateType == CombatantStatusUpdateType.EndRound)
        {
            var stat = context.Combatant.Stats.SingleOrDefault(x => x.Type == _statType);

            if (stat is null)
            {
                IsExpired = true;
                return;
            }

            if (stat.Value.GetShare() < _minShare)
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
}