namespace CombatDicesTeam.Combats;

public sealed record CombatMovementEffectConfig(IReadOnlyCollection<IEffect> Effects,
    IReadOnlyCollection<IEffect> AutoDefenseEffects)
{
    public static CombatMovementEffectConfig Create(IReadOnlyCollection<IEffect> effects)
    {
        return new CombatMovementEffectConfig(effects, ArraySegment<IEffect>.Empty);
    }
}