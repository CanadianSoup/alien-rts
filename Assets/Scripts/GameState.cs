public abstract class GameState
{
    protected GameController gameController;
    protected GameStateMachine gameStateMachine;

    protected GameState(GameController gameController, GameStateMachine gameStateMachine)
    {
        this.gameController = gameController;
        this.gameStateMachine = gameStateMachine;
    }

    protected void DisplayOnUI()
    {
        UIManager.Instance.Display(this);
    }

    public virtual void Enter()
    {
        DisplayOnUI();
    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }

}
