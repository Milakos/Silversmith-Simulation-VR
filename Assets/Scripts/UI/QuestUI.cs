using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public TMP_Text textHeadLine, textDescription;

    private void Start()
    {
        Activate(false);
    }

    public void Activate(bool isActive) 
    {
        this.gameObject.SetActive(isActive);
    }
    public void Quest(string name, string description)
    {
        textHeadLine.text = name;
        textDescription.text = description;
    }
}
