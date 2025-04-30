using UnityEngine;
public  class HammerState : PlayerStateMachine
{
    
    public bool isInHammerState = false;

    public HammerState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInHammerState = true;
    }

    public override void Exit()
    {
        isInHammerState = false;
    }

    public override void Tick()
    {
    }
}
public class HammeringQuestHandler : QuestBase<HammerState>
{
    [SerializeField] GameObject Ingot;
    Vector3 finalScale;    
    public Quest.Achivement hammeringSilver;
    bool isInState => stateMachine.currentState is HammerState hammerState && hammerState.isInHammerState;
    [SerializeField] private Material m_CarvedSilver;
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

    public override void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Hammer"))
        {
            other.gameObject.TryGetComponent(out InteractableManager component);
            component.InteractableAudio(surfaceType, other.contacts[0]); 
            
            if (hit < overallHits)
            {
                hit++;
                // AudioEvent();
                if (hit >= overallHits)
                {
                    CalculateIngotScale();     
                    QuestInProgress();
                }
            }
            if (totalAmountLeft == 0)
            {
                if (isInState)
                {
                    Ingot.TryGetComponent(out Collider col);
                    col.isTrigger = true;
                    EndQuest();
                }
            }
            if (objectCounter < objectsToSpawn.Length - 1)
                objectCounter++;

            print("Hammer Interacted With Ingot");
        }
        else
        {
            if (isInState)
            {
                if(other.gameObject.layer == 10 && !other.collider.CompareTag("Hammer"))
                {
                    AudioEventInteractable(other);
                    print($"{other}Collided with Ingot");
                } 
            }
        }

        base.OnCollisionEnter(other);
    }

    // public override void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Hammer"))
    //     {
    //         if (hit < overallHits)
    //         {
    //             hit++;
    //             AudioEvent();
    //             CalculateIngotScale();                
    //             
    //             if (hit >= overallHits)
    //             {
    //                 QuestInProgress();
    //             }
    //         }
    //         if (totalAmountLeft == 0)
    //         {
    //             EndQuest();
    //             Ingot.TryGetComponent(out Collider col);
    //             col.isTrigger = true;
    //             Ingot.TryGetComponent(out Rigidbody rb);
    //             rb.isKinematic = true;
    //         }
    //         if (objectCounter < objectsToSpawn.Length - 1)
    //             objectCounter++;
    //
    //         print("Hammer Interacted With Ingot");
    //     }
    //     base.OnTriggerEnter(other);
    // }
    public override void StartQuest()
    {
        base.StartQuest();
    }
    public override void QuestInProgress()
    {
        if (isInState)
        {
            particles.Play();
            AudioReward();    
            print("You Hammer Silver Ingot");
            hit = 0;
            totalAmountLeft--;
        }
        base.QuestInProgress();
    }
    public override void EndQuest()
    {
        // Ingot.GetComponent<MeshRenderer>().material = m_CarvedSilver;
        hammeringSilver?.Invoke(quest, true, Reward);
        stateMachine.SwitchState(new CoolDownWaterState(stateMachine));
        
        base.EndQuest();
    }
    void CalculateIngotScale() 
    {
        finalScale = new Vector3(0.03f, -0.001f, 0.05f);
        Ingot.transform.localScale += finalScale;
    }
    public void AudioEventInteractable(Collision other)
    {
        other.gameObject.TryGetComponent(out InteractableManager component);
        component.InteractableHandleAudio(surfaceType);
    }
    public override void AudioEvent()
    {
        base.AudioEvent();
    }
}
