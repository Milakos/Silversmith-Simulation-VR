using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public  class LightFireState : PlayerStateMachine
{
    
    public bool isInFireState= false;

    public LightFireState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInFireState = true;
    }

    public override void Exit()
    {
        isInFireState = false;
    }

    public override void Tick()
    {
    }
}
public class LightFireHandler : QuestBase<LightFireState>
{
    [SerializeField] private XRSocketInteractor[] sockets;
    private List<XRSocketInteractor> XRSockets = new List<XRSocketInteractor>();
    public Quest.Achivement woodAchieved;
    bool isInState => stateMachine.currentState is LightFireState LightFireState && LightFireState.isInFireState;
    public override void Awake()
    {
        base.Awake();

        foreach (XRSocketInteractor socket in sockets) 
        {
            XRSockets.Add(socket);
        }
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
    public void RemoveItemFromList(XRSocketInteractor socketItem) 
    {
        totalAmountLeft--;
        XRSockets.Remove(socketItem);
        AudioEvent();
        if (XRSockets.Count == 0 && totalAmountLeft == 0) 
        {
            EndQuest();
        }
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
            AudioReward();
            particles.Play();
            woodAchieved?.Invoke(quest, true, Reward);
            stateMachine.SwitchState(new PlaceCauldronState(stateMachine));
            print("No more items");          
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
