namespace CombatDicesTeam.Combats;

public class CombatField
{
    public CombatField()
    {
        HeroSide = new CombatFieldSide();
        MonsterSide = new CombatFieldSide();
    }

    public CombatFieldSide HeroSide { get; }
    public CombatFieldSide MonsterSide { get; }
}