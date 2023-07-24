namespace CombatDicesTeam.Combats;

public sealed class CombatMovement
{
    public CombatMovement(CombatMovementSid sid, CombatMovementCost cost, CombatMovementEffectConfig effectConfig)
    {
        Sid = sid;
        Cost = cost;
        Effects = effectConfig.Effects;
        AutoDefenseEffects = effectConfig.AutoDefenseEffects;
    }

    public IReadOnlyCollection<IEffect> AutoDefenseEffects { get; }
    public CombatMovementCost Cost { get; }
    public IReadOnlyCollection<IEffect> Effects { get; }
    public CombatMovementSid Sid { get; }
    public CombatMovementTags Tags { get; init; }
}