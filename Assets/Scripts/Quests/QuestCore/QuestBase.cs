using FMODUnity;
using UnityEngine;
using Zenject;


/// <summary>
/// It is required from any gameobject that will inherit logic from this class to has a boxCollider componenet 
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class QuestBase<TStateMachine> : MonoBehaviour, IQuest where TStateMachine : PlayerStateMachine
{
    /// <summary>
    /// A Quest Base that will be inherited from diferent quest Handlers 
    /// in the game such as quest scriptable object, a reward, a particle system
    /// an audio mechanism, objects that will be spawned or will be activated, particle systems, etc..
    /// </summary>
    [Header("Quest Properties")][Space(10)]
    
    [Tooltip("Insert a Quest of type Scriptable Object")]   
    [SerializeField] public QuestSO quest;
    
    [Space(10)]
    [Tooltip("Select the surface type for this object")]
    public SurfaceType surfaceType;
    
    [Space(10)]
    [Tooltip("Insert a game object as a reward that will be activated after quest is completed if it is needed. it is not mandadory")] 
    [SerializeField] public GameObject Reward;
    
    [Space(10)]
    [Tooltip("Insert a Particle Syste, that will be activated after any logic completed if it is needed. it is not mandadory")]
    [SerializeField] protected ParticleSystem particles;
    
    [Space(10)]
    [SerializeField] protected GameObject lightObject;
    
    [Space(10)]
    [Tooltip("Insert game objects as part of the game mechanic logic that will be activated when they needed. it is not mandadory")]
    [SerializeField] protected GameObject[] objectsToSpawn;
    
    [Space(10)]
    [Tooltip("The maximum amount that will be needed by hitting object")]
    [SerializeField] protected int overallHits = 15;
    [field: SerializeField] public EventReference TriggerEnter { get; private set; }
    [field: SerializeField] public EventReference RewardSound { get; private set; }
    [Inject] protected IAudioManager audioManager;
    //Various counters for mathematical implementation
    protected int hit = 0;
    protected int totalAmountLeft;
    protected int objectCounter = 0;
    public bool Trigger = false;
    
    

    protected BaseStateMachine stateMachine;
    /// <summary>
    /// Inject the BaseStateMachine dependency.
    /// </summary>
    [Inject]
    public void Construct(BaseStateMachine injectedStateMachine)
    {
        stateMachine = injectedStateMachine;
    }

    /// <summary>
    /// In the awake method we evaluate the box collider and the boolean isTrigger as true
    /// to prevent it from collision and physics event with colliders
    /// Also setting the total amount that needed to be achieved as equal to the target amount of each 
    /// quest scriptable object that is attached
    /// </summary>
    public virtual void Awake()
    {
        totalAmountLeft = quest.targetAmount;
        GetComponent<BoxCollider>().isTrigger = Trigger;
    }
    /// <summary>
    /// checking null refrences of the reward and setting it as a non active object.
    /// Also setting inactive all objects that will be needed after.
    /// </summary>
    public virtual void Start()
    {
        StartQuest();

        if (Reward != null)
            Reward.SetActive(false);
        if (lightObject != null)
            lightObject.SetActive(false);
        
        foreach (GameObject obj in objectsToSpawn)
        {
            SpawnObject(obj, false);
        }
    }
    public virtual void Update() {}
    /// <summary>
    /// A virtual OntiggerEnterMethod that checks if the objects that overrides this method collides
    /// with an another collider with a specific tag that user will implement at the inspector
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnter(Collider other){ }
    public virtual void OnCollisionEnter(Collision other) {}
    /// <summary>
    /// A method that is responsible for setting active or inactive the objects that will be spawned
    /// also is first called at the start method
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="visible"></param>
    public virtual void SpawnObject(GameObject obj, bool visible)
    {
        obj.SetActive(visible);
    }

    public virtual void StartQuest(){}

    public virtual void QuestInProgress(){}

    public virtual void EndQuest()
    {
        // Use the injected AudioManager to play the quest complete sound.
        audioManager?.PlayOneShot(audioManager.QuestCompleteEvent, transform.position);
    }
    public virtual void AudioEvent() 
    {
        audioManager?.PlayOneShot(TriggerEnter, this.transform.position);      
    }
    public virtual void AudioReward() 
    {
        audioManager?.PlayOneShot(RewardSound, this.transform.position);
    }

}

