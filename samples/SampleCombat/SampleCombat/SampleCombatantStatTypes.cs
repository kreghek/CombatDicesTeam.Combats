using CombatDicesTeam.Combats;

namespace SampleCombat;

public static class SampleCombatantStatTypes
{
    public static ICombatantStatType HP { get; } = new CombatantStatType("HP");
}