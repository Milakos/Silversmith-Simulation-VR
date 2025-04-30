using UnityEngine;
public  class CarvingSilverSheetState : PlayerStateMachine
{
    
    public bool isInCarvingState = false;

    public CarvingSilverSheetState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInCarvingState = true;
    }

    public override void Exit()
    {
        isInCarvingState = false;
    }

    public override void Tick()
    {

    }
}
public class CarvingSilverSheetHandler : QuestBase<CarvingSilverSheetState>
{
    public Quest.Achivement carvAchieved;
    [SerializeField] GameObject Ingot;
    [SerializeField] private Material m_CarvedSilver;
    public bool isInState => stateMachine.currentState is CarvingSilverSheetState carvingState && carvingState.isInCarvingState;
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
        if (other.CompareTag("ScrewDriver")) 
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
        Ingot.GetComponent<MeshRenderer>().material = m_CarvedSilver;
        carvAchieved?.Invoke(quest, true, Reward);
        stateMachine.SwitchState(new CutWithSawState(stateMachine));
        base.EndQuest();
    }
    public void AudioEventInteractable(Collider other)
    {
        other.gameObject.TryGetComponent(out InteractableManager component);
        component.InteractableHandleAudio(surfaceType);
    }
    public override void AudioEvent()
    {
        base.AudioEvent();
    }
}
