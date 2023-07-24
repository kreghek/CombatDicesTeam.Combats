using CombatDicesTeam.Utils;

namespace CombatDicesTeam.Combats.Effects;

public interface IDamageEffectModifier
{
    Range<int> Process(Range<int> damage);
}