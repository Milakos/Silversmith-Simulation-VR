using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingHands : MonoBehaviour
{
    /// <summary>
    /// A Class that is implemented in each hand at the grabbable objects 
    /// that defines which hand is left and which hand is right
    /// </summary>
    public enum ChooseHand
    {
        Right,
        Left
    }
    public ChooseHand choose;
    NamingHands instance;
    private void Awake()
    {
        instance = this;       
    }
}