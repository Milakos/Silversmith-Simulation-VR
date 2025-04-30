using UnityEngine;
public class TreeState : PlayerStateMachine
{
    public bool isInTreeState = false;

    public TreeState(BaseStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    
    public override void Enter()
    {
        isInTreeState = true;
        Debug.Log($"Enter {nameof(TreeState).ToString()}");
    }

    public override void Exit()
    {
        isInTreeState = false;
        Debug.Log($"Exit {nameof(TreeState).ToString()}");
    }

    public override void Tick()
    {
    }
}
[RequireComponent(typeof(BoxCollider))]
public class TreeHandler : QuestBase<TreeState>
{
    /// <summary>
    /// The TreeHandler class is responsible for checking if an object with the tag Axe is collides with this game object
    /// If yes, then calculates the hits that collides and implementing different logic. When collides it counts the hit,
    /// then when the hits are surpasing the overall hits that needed to be counted for spawning the collectable, the collectable 
    /// will be enabled as a particle system, and will reset the hits. When this logic completed the total amount of tries reduced 
    /// as the list of objects. when everything is completed the event of completedaction is invoked to the subscriber gustGiver to
    /// change to the next quest.
    /// </summary>
    /// 
    public Quest.Achivement treeAchieved;
    bool isInState => stateMachine.currentState is TreeState treeState && treeState.isInTreeState;
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
        if (other.collider.CompareTag("Axe"))
        {       
            other.gameObject.TryGetComponent<InteractableManager>(out InteractableManager component);
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
                {
                    EndQuest();
                }
            }
            if (objectCounter < objectsToSpawn.Length - 1)
                objectCounter++;
            print("Axe Collided with Tree");
            
        }
        else
        {
            if(other.gameObject.layer == 10 && !other.collider.CompareTag("Axe"))
            {
                AudioEventInteractable(other);
                print($"{other}Collided with Tree");
            }         
        }
        base.OnCollisionEnter(other);
    }
    // public override void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Axe"))
    //     {
    //         if (hit < overallHits)
    //         {
    //             hit++;
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
    //         print("Axe Triggered with Tree");
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
            AudioReward();
            particles.Play();            
            base.SpawnObject(objectsToSpawn[objectCounter], true);
            print("You take Tree wood branches");
            hit = 0;
            totalAmountLeft--;                      
        }
        
        base.QuestInProgress();  
    }
    public override void EndQuest()
    {
        treeAchieved?.Invoke(quest, true, null);
        stateMachine.SwitchState(new LightFireState(stateMachine));
        base.EndQuest();
    }
    public override void AudioReward()
    {
        base.AudioReward();
    }
    public void AudioEventInteractable(Collision other)
    {
        other.gameObject.TryGetComponent(out InteractableManager component);
        component.InteractableHandleAudio(surfaceType);
    }
}
