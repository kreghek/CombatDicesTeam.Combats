namespace CombatDicesTeam.Combats;

public interface IEffectInstance
{
    ITargetSelector Selector { get; }
    void AddModifier(IStatModifier modifier);
    void Influence(ICombatant target, IStatusCombatContext context);
    void RemoveModifier(IStatModifier modifier);
}