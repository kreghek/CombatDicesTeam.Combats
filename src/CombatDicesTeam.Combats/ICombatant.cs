namespace CombatDicesTeam.Combats;

public interface ICombatant
{
    bool IsDead { get; }

    ITeam Team { get; }

    void UpdateEffects(CombatantEffectUpdateType updateType,
        ICombatantEffectLifetimeDispelContext effectLifetimeDispelContext);

}