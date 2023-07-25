using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public interface IDamageEffectModifier
{
    GenericRange<int> Process(GenericRange<int> damage);
}