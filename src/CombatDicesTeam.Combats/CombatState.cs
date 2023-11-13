namespace CombatDicesTeam.Combats;

/// <summary>
/// Base implementation of combat state.
/// </summary>
public class CombatState : ICombatState
{
    /// <summary>
    /// Constructor of state object.
    /// </summary>
    /// <param name="isFinalState">Is state finish the combat.</param>
    public CombatState(bool isFinalState = true)
    {
        IsFinalState = isFinalState;
    }

    /// <inheritdoc />
    public bool IsFinalState { get; }
}