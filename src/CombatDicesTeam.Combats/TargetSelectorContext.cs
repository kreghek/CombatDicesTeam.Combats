using CombatDicesTeam.Dices;

namespace CombatDicesTeam.Combats;

public sealed class TargetSelectorContext : ITargetSelectorContext
{
    private readonly IDictionary<ITargetSelectorContextCombatantType, List<ICombatant>> _combatants;

    public TargetSelectorContext(CombatFieldSide actorSide, CombatFieldSide enemySide, IDice dice, ICombatant? Attacker)
    {
        ActorSide = actorSide;
        EnemySide = enemySide;
        Dice = dice;

        _combatants = new Dictionary<ITargetSelectorContextCombatantType, List<ICombatant>>
        {
            {  }
        };
    }

    public CombatFieldSide ActorSide { get; }
    public CombatFieldSide EnemySide { get; }
    public IDice Dice { get; }

    public IReadOnlyCollection<ICombatant> GetCombatants(ITargetSelectorContextCombatantType combatantType)
    {
        throw new NotImplementedException();
    }
}