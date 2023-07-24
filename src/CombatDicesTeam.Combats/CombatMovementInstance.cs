namespace Core.Combats;

public sealed class CombatMovementInstance
{
    public CombatMovementInstance(CombatMovement sourceMovement)
    {
        SourceMovement = sourceMovement;
        Effects = sourceMovement.Effects.Select(x => x.CreateInstance()).ToArray();
        AutoDefenseEffects = sourceMovement.AutoDefenseEffects.Select(x => x.CreateInstance()).ToArray();

        Cost = new CombatMovementCost(new StatValue(sourceMovement.Cost.Amount.Current));
    }

    public IReadOnlyCollection<IEffectInstance> AutoDefenseEffects { get; }

    public CombatMovementCost Cost { get; }

    public IReadOnlyCollection<IEffectInstance> Effects { get; }

    public CombatMovement SourceMovement { get; }
}