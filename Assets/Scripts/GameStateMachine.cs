public class GameStateMachine
{
    public GameState CurrentState { get; private set; }

    public void Initialize(GameState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState.Exit();

        CurrentState = newState;
        newState.Enter();
    }
}
