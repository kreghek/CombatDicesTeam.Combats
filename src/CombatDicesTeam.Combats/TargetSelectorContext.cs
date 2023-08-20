using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public sealed class TargetSelectorContext : ITargetSelectorContext
{
    private readonly IDictionary<ITargetSelectorContextCombatantType, List<ICombatant>> _combatants;

    public TargetSelectorContext(CombatFieldSide actorSide, CombatFieldSide enemySide, IDice dice, ICombatant? attacker)
    {
        ActorSide = actorSide;
        EnemySide = enemySide;
        Dice = dice;

        _combatants = new Dictionary<ITargetSelectorContextCombatantType, List<ICombatant>>
        {
            { TargetSelectorContextCombatantTypes.Attacker, new List<ICombatant>() }
        };

        if (attacker != null)
        {
            _combatants[TargetSelectorContextCombatantTypes.Attacker].Add(attacker);
        }
    }

    public CombatFieldSide ActorSide { get; }
    public CombatFieldSide EnemySide { get; }
    public IDice Dice { get; }

    public IReadOnlyCollection<ICombatant> GetCombatants(ITargetSelectorContextCombatantType combatantType)
    {
        return _combatants[TargetSelectorContextCombatantTypes.Attacker];
    }
}