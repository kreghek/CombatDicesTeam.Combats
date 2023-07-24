namespace Core.Combats.Effects;

using CombatDicesTeam.Utils;

public interface IDamageEffectModifier
{
    Range<int> Process(Range<int> damage);
}