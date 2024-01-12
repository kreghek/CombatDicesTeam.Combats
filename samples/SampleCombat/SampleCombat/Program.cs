using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;

using SampleCombat;

using Spectre.Console;

var combat = new SampleCombatEngine(new LinearDice(), new SampleRoundQueueResolver(), new EliminatingCombatStateStrategy());

combat.Initialize(new[]
{
    new FormationSlot(0,0)
    {
        new SampleCombatant("warrior", new []
        {
            new CombatMovementInstance(new CombatMovement(new CombatMovementSid("hit"), new CombatMovementCost(0), CombatMovementEffectConfig.Create(new []
            {
                new DamageEffect()
            })))
        })
    }
},
    new []{new FormationSlot(0,0)});

while (!combat.StateStrategy
           .CalculateCurrentState(new CombatStateStrategyContext(combat.CurrentCombatants, combat.CurrentRoundNumber))
           .IsFinalState)
{
    foreach (var combatant in combat.CurrentCombatants)
    {
        AnsiConsole.WriteLine(combatant.ClassSid);   
    }
}
