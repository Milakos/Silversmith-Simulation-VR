using UnityEngine;
public  class PlaceCauldronState : PlayerStateMachine
{  
    public bool isInPlaceCauldronState = false;

    public PlaceCauldronState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInPlaceCauldronState = true;
    }

    public override void Exit()
    {
        isInPlaceCauldronState = false;
    }

    public override void Tick()
    {
    }
}
public class PlaceCauldronHandler : QuestBase<PlaceCauldronState>
{
    public Quest.Achivement PotPlacedAchievement;
    public GameObject newCauldron;
    bool isInState => stateMachine.currentState is PlaceCauldronState placeCauldronState && placeCauldronState.isInPlaceCauldronState;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void StartQuest()
    {
        base.StartQuest();
    }
    public override void QuestInProgress()
    {
        base.QuestInProgress();
    }
    public override void EndQuest()
    {
        if(isInState)
        {
            lightObject.SetActive(true);
            AudioEvent();
            AudioReward();
            newCauldron.SetActive(true);
            PotPlacedAchievement?.Invoke(quest, true, Reward);
            stateMachine.SwitchState(new RockState(stateMachine));
        }
        base.EndQuest();
    }
    public override void AudioEvent()
    {
        base.AudioEvent();
    }
    public override void AudioReward()
    {
        base.AudioReward();
    }
}
