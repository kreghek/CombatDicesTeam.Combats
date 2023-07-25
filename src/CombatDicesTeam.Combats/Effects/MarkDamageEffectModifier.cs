using CombatDicesTeam.GenericRanges;

namespace CombatDicesTeam.Combats.Effects;

public sealed class MarkDamageEffectModifier : IDamageEffectModifier
{
    private readonly int _bonus;

    public MarkDamageEffectModifier(int bonus)
    {
        _bonus = bonus;
    }

    public GenericRange<int> Process(GenericRange<int> damage)
    {
        return new GenericRange<int>(damage.Min + _bonus, damage.Max + _bonus);
    }
}