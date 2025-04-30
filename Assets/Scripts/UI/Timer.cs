using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// A Class that is responsible for calculating and displaying the remaining time
    /// Also checks and gives the event when the action is completed to jump to the next quest
    /// </summary>
    private float timeRemaining = 10;
    private bool timerIsRunning = false;

    [SerializeField] private TMP_Text timeText;
    public Quest.Achivement smeltAchieved;
    
    SmelterHandler smelterHandler;

    [SerializeField] GameObject existedCauldron;
    [SerializeField] GameObject newCauldron;

    private void Awake()
    {
        smelterHandler = FindObjectOfType<SmelterHandler>();
    }
    private void OnEnable()
    {
        timerIsRunning = true;
    }
    /// <summary>
    /// OnENable
    /// Boolean check for timer to be active and run when the gameobject is enabled by another script
    /// 
    /// Update
    /// In the Update method it is calculated when the timer is bigger than zero to decreasing and when this 
    /// comes at zero value, stops the timer, and sends the event to the quest giver subscriber
    /// </summary>
    void Update()
    {
        
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                if (smelterHandler.quest == FindObjectOfType<QuestGiver>().currentQuest[0])
                {
                    if (smeltAchieved != null)
                    {
                        existedCauldron.SetActive(false);
                        newCauldron.SetActive(true);
                        
                        smeltAchieved(smelterHandler.quest, true, smelterHandler.Reward);
                    }
                    Destroy(this.gameObject);
                }
            }
        }
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