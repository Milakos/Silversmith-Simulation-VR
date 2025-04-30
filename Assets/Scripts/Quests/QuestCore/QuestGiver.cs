using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private Animator anim;
    // Index Counter of the current quest
    private int currentQuestIndex = 0;
    // Index that counts the next quest
    private int nextIndex = 0;
    // List of all quest in the game
    [SerializeField] private List<QuestSO> quests = new List<QuestSO>();
    // List that will only contain one element that will be added and removed
    //showing the current Quest
    public List<QuestSO> currentQuest = new List<QuestSO>();
    public QuestSO final;
    public QuestUI questDisplay;

    private void OnEnable()
    {
        FindObjectOfType<TreeHandler>().treeAchieved += UpdateQuestProgress;
        FindObjectOfType<RockHandler>().rocksAchieved += UpdateQuestProgress;
        FindObjectOfType<LightFireHandler>().woodAchieved += UpdateQuestProgress;
        FindObjectOfType<PlaceCauldronHandler>().PotPlacedAchievement += UpdateQuestProgress;
        FindObjectOfType<HammeringQuestHandler>().hammeringSilver += UpdateQuestProgress;
        FindObjectOfType<CoolDownWater>().coolDownSilver += UpdateQuestProgress;
        FindObjectOfType<SmelterHandler>().smeltAchieved += UpdateQuestProgress;
        FindObjectOfType<CarvingSilverSheetHandler>().carvAchieved += UpdateQuestProgress;
        FindObjectOfType<CutWithSawHandler>().cutAchieved += UpdateQuestProgress;
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        /*player = FindObjectOfType<GameObject>();*/
        foreach (QuestSO quest in quests)
        {
            quest.isCompleted = false;
        }
        currentQuest.Add(quests[currentQuestIndex]);
        questDisplay.Quest(currentQuest[currentQuestIndex].questName, currentQuest[currentQuestIndex].description);
  
    }

    /// <summary>
    /// A region that is responsible of displaying the quest canvas when the player 
    /// comes in - out in the specific area
    /// </summary>
    /// <param name="other"></param>
    #region TriggerCollider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            questDisplay.Activate(true);
            anim.SetBool("talk", true);
        }       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questDisplay.Activate(false);
            anim.SetBool("talk", false);
        }
    }
    #endregion TriggerCollider


    /// <summary>
    /// A method that is responsible for the transition of quest and display UI elements depend on each quest
    /// </summary>
    /// <param name="quest"></param>
    /// <param name="completed"></param>
    /// <param name="reward"></param>
    public void UpdateQuestProgress(QuestSO quest, bool completed, GameObject reward)
    {
        quest.isCompleted = completed;

        if (quest == currentQuest[currentQuestIndex] && completed)
        {
            if (reward != null)
                reward.SetActive(true);

            currentQuest.Remove(quest);

            if (nextIndex < quests.Count)
                nextIndex++;

            if (HasQuestsRemain())
            {
                currentQuest.Add(quests[nextIndex]);
                questDisplay.Quest(quests[nextIndex].questName, quests[nextIndex].description);
                print("New Quest" + quests[nextIndex].questName);
            }
            else
            {
                print("No more Quests");
            }
        }
        else
        {
            questDisplay.Quest(final.questName, final.description);
            print("You Finish");
        }
    }
    /// <summary>
    /// Function that returns true or false depend if there is another quest to complete
    /// </summary>
    /// <returns></returns>
    public bool HasQuestsRemain() 
    {
        if (nextIndex < quests.Count)
        {
            return true;
        }
        else 
        {
            return false;            
        }
    }
}
