using UnityEngine;
public  class CutWithSawState : PlayerStateMachine
{
    
    public bool isInCutState = false;

    public CutWithSawState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInCutState = true;
    }

    public override void Exit()
    {
        isInCutState = false;
    }

    public override void Tick()
    {
    }
}
public class CutWithSawHandler : QuestBase<CutWithSawState>
{
    public Quest.Achivement cutAchieved;
    [SerializeField] GameObject silverSheet;
    public bool isInState => stateMachine.currentState is CutWithSawState cutWithSawState && cutWithSawState.isInCutState;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void StartQuest()
    {
        base.StartQuest();
    }
    public override void QuestInProgress()
    {
        hit = 0;
        totalAmountLeft--;
        base.QuestInProgress();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saw"))
        {
            if (hit < overallHits)
            {
                hit++;
                AudioEvent();
                if (hit >= overallHits)
                {
                    QuestInProgress();
                }
            }
            if (totalAmountLeft == 0)
            {
                EndQuest();
            }
        }
        else
        {
            if(isInState)
            {
                if(other.gameObject.layer == 10)
                {
                    AudioEventInteractable(other);
                } 
            }
        }
        base.OnTriggerEnter(other);
    }
    public override void EndQuest()
    {
        silverSheet.SetActive(false);
        particles.Play();
        if(lightObject != null)
            lightObject.SetActive(true);
        cutAchieved?.Invoke(quest, true, Reward);
        base.EndQuest();
    }
    public override void AudioEvent()
    {
        base.AudioEvent();
    }
    public void AudioEventInteractable(Collider other)
    {
        other.gameObject.TryGetComponent(out InteractableManager component);
        component.InteractableHandleAudio(surfaceType);
    }
}
