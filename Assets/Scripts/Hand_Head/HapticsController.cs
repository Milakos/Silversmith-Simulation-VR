using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticsController : MonoBehaviour
{
    /// <summary>
    /// A Class that is responsible for adding extra functionality when the hand interactors hovers above 
    /// specific object interactables
    /// </summary>
    [SerializeField] private ActionBasedController leftController, rightController;
    
    [Header("Haptics Properties")]
    [SerializeField] private float hapticsAmplitude, hapticsDuration;
    
    [ContextMenu("Send Haptics")] 
    public void SendHaptics(bool isLeft, float amplitude, float duration)
    {    
        hapticsAmplitude = amplitude;
        hapticsDuration = duration;
        
        if (isLeft)
        {
            leftController.SendHapticImpulse(hapticsAmplitude, hapticsDuration);
        }
        else
        {
            rightController.SendHapticImpulse(hapticsAmplitude, hapticsDuration);
        }       
    }


}
