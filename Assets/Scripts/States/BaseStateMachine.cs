public class BaseStateMachine : StateMachine
{
    private void Start() 
    {
        SwitchState(new TreeState(this));
        // SwitchState(new CoolDownWaterState(this));
    }
}
