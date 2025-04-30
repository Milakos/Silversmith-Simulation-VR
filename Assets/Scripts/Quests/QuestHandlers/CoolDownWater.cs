using UnityEngine;
public class CoolDownWaterState : PlayerStateMachine
{
    
    public bool isInCoolDown = false;

    public CoolDownWaterState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInCoolDown = true;
    }

    public override void Exit()
    {
        isInCoolDown = false;
    }

    public override void Tick()
    {
    }
}
public class CoolDownWater : QuestBase<CoolDownWaterState>
{
    public Quest.Achivement coolDownSilver;
    [SerializeField] private Material m_CarvedSilver;
    [SerializeField] private GameObject metalSheet;
    [SerializeField] private GameObject metalSheetSocket2;
    public bool isInState => stateMachine.currentState is CoolDownWaterState coolDownWaterState && coolDownWaterState.isInCoolDown;
    
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
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tongs"))
        {
            if (isInState)
            {
                QuestInProgress();
            }
        }
        AudioEvent();
        base.OnTriggerEnter(other);
    }
    public override void StartQuest()
    {
        base.StartQuest();
    }
    public override void QuestInProgress()
    {
        // particles.Play();
        print("You Cooldown Metal");
        metalSheetSocket2.SetActive(true);
        metalSheet.GetComponent<MeshRenderer>().material = m_CarvedSilver;
        base.QuestInProgress();
    }
    public override void EndQuest()
    {
        coolDownSilver?.Invoke(quest, true, Reward);
        stateMachine.SwitchState(new CarvingSilverSheetState(stateMachine));
        metalSheetSocket2.TryGetComponent(out BoxCollider boxCollider);
        boxCollider.enabled = false;
        base.EndQuest();
    }

    public void OnSelectAndAttach(InteractableManager tongs)
    {
        tongs.StoreInInventory();
    }

    public override void AudioEvent()
    {
        base.AudioEvent();
    }
}
