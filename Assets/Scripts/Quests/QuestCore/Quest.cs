using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public delegate void Achivement(QuestSO quest, bool completed, GameObject reward);
}
