using TMPro;
using UnityEngine;
public  class SmelterState : PlayerStateMachine
{
    
    public bool isInSmelterState = false;

    public SmelterState(BaseStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        isInSmelterState = true;
    }

    public override void Exit()
    {
        isInSmelterState = false;
    }

    public override void Tick() { }
}
public class SmelterHandler : QuestBase<SmelterState>
{  
    public GameObject timer;
    /// <summary>
    /// A Class that is responsible for calculating and displaying the remaining time
    /// Also checks and gives the event when the action is completed to jump to the next quest
    /// </summary>
    private float timeRemaining = 10;
    private bool timerIsRunning = false;

    [SerializeField] private TMP_Text timeText;
    public Quest.Achivement smeltAchieved;
    public bool isOreAttached = false;
    bool isInState => stateMachine.currentState is SmelterState smeltState && smeltState.isInSmelterState;

    [SerializeField] GameObject existedCauldron;
    [SerializeField] GameObject newCauldron;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        
        base.Start();
    }

    public void OreAttached(bool value)
    {
        isOreAttached = value;
    }

    public override void Update()
    {
        if (isInState)
        {
            if (isOreAttached)
            {
                timer.SetActive(true);
                
                if (timerIsRunning)
                {
                    QuestInProgress();
                } 
            }
        }
    }

    public override void StartQuest()
    {
        timerIsRunning = true;
    }
    public override void QuestInProgress()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            timerIsRunning = false;

            newCauldron.GetComponent<Animator>().SetBool("Move", true);

            EndQuest();
        }
        // base.QuestInProgress();
    }
    public override void EndQuest()
    {
        // if (timer != null)
        //     timer.SetActive(true);
        smeltAchieved?.Invoke(quest, true, Reward);
        stateMachine.SwitchState(new HammerState(stateMachine));
        timer.SetActive(false);
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
    /// <summary>
    /// Method to Display decreasing time counter in the UI. 
    /// </summary>
    /// <param name="timeToDisplay"></param>
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
