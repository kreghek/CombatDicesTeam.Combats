using CombatDicesTeam.Utils;

namespace CombatDicesTeam.Combats.Effects;

public sealed class MarkDamageEffectModifier : IDamageEffectModifier
{
    private readonly int _bonus;

    public MarkDamageEffectModifier(int bonus)
    {
        _bonus = bonus;
    }

    public Range<int> Process(Range<int> damage)
    {
        return new Range<int>(damage.Min + _bonus, damage.Max + _bonus);
    }
}