using UnityEngine;


public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject obj;
/*    [SerializeField] private TMP_Text textTeleport;*/

    private void Awake() 
    {
/*        textTeleport = GetComponentInChildren<TMP_Text>();*/
    }
    private void Start() 
    {
        obj.SetActive(false);
    }
    private void OnEnable() 
    {
/*        
        FindObjectOfType<TeleportSwitch>().changeButtonText += ChangeToggleText;*/
        FindObjectOfType<ButtonActionsController>().activateCanvasUI += ActivateUICanvas;    
    }
    private void OnDisable()
    {
        // FindObjectOfType<ButtonActionsController>().activateCanvasUI -= ActivateUICanvas;  
    }
/*    public void ChangeToggleText(bool changeText)
    {
        if(changeText == true)
            textTeleport.text = "Teleport";
        else
            textTeleport.text = "Interact";
    }*/
    
    public void ActivateUICanvas(bool i)
    {
        if(i == true)
            obj.SetActive(true);
        else
            obj.SetActive(false);
    }



}
