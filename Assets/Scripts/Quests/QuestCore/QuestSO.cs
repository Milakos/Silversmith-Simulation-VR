using UnityEngine;

public enum SurfaceType
{
    None,
    Wood,
    Rock,
    Glass,
    Metal,
    Floor,
    Water
}
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 1)]
public class QuestSO : ScriptableObject
{
    [Header("Quest Name")]
    public string questName;
    
    [Header("Quest Description")]
    [Tooltip("Text that will be displayed as comment in the inventory")]
    [TextArea(0, 20)] public string description;
    

    [Header("Quest Type")]
    public QuestType type;
    public enum QuestType { CollectWoodBranches, CollectSilverOre, SmeltOre }

    [Header("Quest Properties")]
    public int targetAmount;   
    public bool isCompleted;
    public void ResetBoolean() 
    {
        isCompleted = !isCompleted;
    }
}
