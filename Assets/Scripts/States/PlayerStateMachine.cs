public abstract class PlayerStateMachine : State
{
    protected new BaseStateMachine stateMachine;
    public PlayerStateMachine(BaseStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
