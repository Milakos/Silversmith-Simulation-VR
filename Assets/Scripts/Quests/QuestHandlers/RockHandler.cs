using UnityEngine;
public  class RockState : PlayerStateMachine
{
    
    public bool isInRockState = false;

    public RockState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInRockState = true;
    }

    public override void Exit()
    {
        isInRockState = false;
    }

    public override void Tick()
    {
    }
}
[RequireComponent(typeof(BoxCollider))]
public class RockHandler : QuestBase<RockState>
{
    public Quest.Achivement rocksAchieved;
    bool isInState => stateMachine.currentState is RockState rockState && rockState.isInRockState;

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void OnCollisionEnter(Collision other) 
    {
        if (other.collider.CompareTag("PickAxe"))
        {
            other.gameObject.TryGetComponent(out InteractableManager component);
            component.InteractableAudio(surfaceType, other.contacts[0]);      

            if (hit < overallHits)
            {
                hit++;

                if (hit >= overallHits)
                {
                    QuestInProgress();
                }
            }
            if (totalAmountLeft == 0)
            {
                if(isInState)
                    EndQuest();
            }
            if (objectCounter < objectsToSpawn.Length - 1)
                objectCounter++;
            
            print("Axe INteracted with Rock");
        }
        else
        {
            if(other.gameObject.layer == 10 && !other.collider.CompareTag("PickAxe"))
            {
                AudioEventInteractable(other);
                print($"{other}Collided with Rock");
            } 
        }
        base.OnCollisionEnter(other);
    }
    // public override void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("PickAxe"))
    //     {
    //         if (hit < overallHits)
    //         {
    //             hit++;
    //             AudioEvent();
    //             if (hit >= overallHits)
    //             {
    //                 QuestInProgress();
    //             }
    //         }
    //         if (totalAmountLeft == 0)
    //         {
    //             EndQuest();
    //         }
    //         if (objectCounter < objectsToSpawn.Length - 1)
    //             objectCounter++;
    //         
    //         print("Axe INteracted with Rock");
    //     }
    // }
    public override void StartQuest()
    {
        base.StartQuest();
    }
    public override void QuestInProgress()
    {
        if(isInState)
        {
            particles.Play();
            base.SpawnObject(objectsToSpawn[objectCounter], true);
            AudioReward();    
            print("You take Silver Ore");
            hit = 0;
            totalAmountLeft--;
        }

        base.QuestInProgress();
    }
    public override void EndQuest()
    {

        rocksAchieved?.Invoke(quest, true, Reward);
        stateMachine.SwitchState(new SmelterState(stateMachine));
        base.EndQuest();
    }
    public override void AudioEvent()
    {
        base.AudioEvent();
    }
    public void AudioEventInteractable(Collision other)
    {
        other.gameObject.TryGetComponent(out InteractableManager component);
        component.InteractableHandleAudio(surfaceType);
    }
    public override void AudioReward()
    {
        base.AudioReward();
    }

}
