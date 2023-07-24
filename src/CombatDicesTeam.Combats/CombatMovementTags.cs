namespace Core.Combats;

[Flags]
public enum CombatMovementTags
{
    None = 0,
    Attack = 1 << 0,
    AutoDefense = 1 << 1
}