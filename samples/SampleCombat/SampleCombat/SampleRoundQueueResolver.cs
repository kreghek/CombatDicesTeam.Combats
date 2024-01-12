using CombatDicesTeam.Combats;

namespace SampleCombat;

public sealed class SampleRoundQueueResolver: IRoundQueueResolver
{
    public IReadOnlyList<ICombatant> GetCurrentRoundQueue(IReadOnlyCollection<ICombatant> combatants)
    {
        return combatants.OrderBy(x => !x.IsPlayerControlled).ToArray();
    }
}

public sealed class SkillTargetSelector : ITargetSelector
{
    private readonly SkillTargetSelectorManager _behaviour;

    public SkillTargetSelector(SkillTargetSelectorManager behaviour)
    {
        _behaviour = behaviour;
    }
    
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return new[] { _behaviour.TargetCombatant ?? throw new Exception() };
    }
}

public sealed class SkillTargetSelectorManager
{
    public ICombatant? TargetCombatant { get; set; }
}

public sealed class UseSkillIntention : IIntention
{
    public UseSkillIntention(ICombatant targetCombatant)
    {
        
    }
    
    public void Make(CombatEngineBase combatEngine)
    {
        throw new NotImplementedException();
    }
}

public sealed class CombatantBehaviour : ICombatActorBehaviour
{
    public void HandleIntention(ICombatantBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        
    }

    public ICombatant? TargetCombatant { get; private set; }
}