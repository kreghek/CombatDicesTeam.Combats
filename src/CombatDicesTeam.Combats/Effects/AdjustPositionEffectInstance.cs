namespace CombatDicesTeam.Combats.Effects;

public sealed class AdjustPositionEffectInstance : EffectInstanceBase<AdjustPositionEffect>
{
    public AdjustPositionEffectInstance(AdjustPositionEffect baseEffect) : base(baseEffect)
    {
    }

    public override void Influence(ICombatant target, ICombatMovementContext context)
    {
        var targetSide = GetTargetSide(target, context.Field);

        var currentCoords = targetSide.GetCombatantCoords(target);

        var heroSide = context.Field.HeroSide;

        var isHeroOnTheList = heroSide[currentCoords with
                              {
                                  ColumentIndex = 0
                              }].Combatant is not null ||
                              heroSide[currentCoords with
                              {
                                  ColumentIndex = 1
                              }].Combatant is not null;

        if (isHeroOnTheList)
        {
            return;
        }

        var isHeroAbove = currentCoords.LineIndex > 0 &&
                          (heroSide[new FieldCoords(ColumentIndex: 0, LineIndex: currentCoords.LineIndex - 1)]
                               .Combatant is not null ||
                           heroSide[new FieldCoords(ColumentIndex: 1, LineIndex: currentCoords.LineIndex - 1)]
                               .Combatant is not null);

        var isHeroBelow = currentCoords.LineIndex < 2 &&
                          (heroSide[new FieldCoords(ColumentIndex: 0, LineIndex: currentCoords.LineIndex + 1)]
                               .Combatant is not null ||
                           heroSide[new FieldCoords(ColumentIndex: 1, LineIndex: currentCoords.LineIndex + 1)]
                               .Combatant is not null);

        if (isHeroAbove)
        {
            context.MoveToPosition(target, currentCoords, targetSide,
                currentCoords with
                {
                    LineIndex = currentCoords.LineIndex - 1
                },
                targetSide,
                new PositionChangeReason());
        }
        else if (isHeroBelow)
        {
            context.MoveToPosition(target, currentCoords, targetSide,
                currentCoords with
                {
                    LineIndex = currentCoords.LineIndex + 1
                },
                targetSide,
                new PositionChangeReason());
        }
        else
        {
            context.MoveToPosition(target, currentCoords, targetSide,
                currentCoords with
                {
                    LineIndex = 1
                },
                targetSide,
                new PositionChangeReason());
        }
    }

    private static CombatFieldSide GetTargetSide(ICombatant target, CombatField field)
    {
        try
        {
            var _ = field.HeroSide.GetCombatantCoords(target);
            return field.HeroSide;
        }
        catch (ArgumentException)
        {
            var _ = field.MonsterSide.GetCombatantCoords(target);
            return field.MonsterSide;
        }
    }
}