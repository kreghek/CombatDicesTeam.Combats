namespace CombatDicesTeam.Combats;

public interface IEffectInstance
{
    ITargetSelector Selector { get; }
    void AddModifier(IUnitStatModifier modifier);
    void Influence(ICombatant target, IStatusCombatContext context);
    void RemoveModifier(IUnitStatModifier modifier);
}