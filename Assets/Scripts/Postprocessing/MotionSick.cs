using UnityEngine;
using UnityEngine.Rendering;


public class MotionSick : MonoBehaviour
{

    public Volume vol;    
    public VolumeComponent vignette;
    void Start()
    {
        vol = GetComponent<Volume>();
        vignette = vol.profile.components[1];
        // vignette.active = false; 
    }
    private void OnEnable() 
    { 
        FindObjectOfType<ButtonActionsController>().MotionSickVignetteTrigger += Motionblur;  
    }
    /// <summary>
    /// Motion blur logic that is visible when the controllers are pressed or are in progress that corresponds
    /// with a boolean check sending it to that class and subscribes as a listener
    /// </summary>
    /// <param name="isInteracting"></param>
    public void Motionblur(bool isInteracting)
    {        
        if (isInteracting == true)
        {
            vignette.active = true;       
        }
        else
        {
            vignette.active = false;         
        }
    }
}
