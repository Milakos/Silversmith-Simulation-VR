using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonActionsController : MonoBehaviour
{
    ActionBasedContinuousMoveProvider mover;
    ActionBasedContinuousTurnProvider turner;
    bool LocomotionNotifier = false;
    /////////////////// /////// Event Declairation \\\\\\\\\\\\\\\\\\\ \\\\\\\\\\\\\\\\\

    // Event with the right Stick for rotation
    public delegate void UI_CanvasActivation(bool i);
    public event UI_CanvasActivation activateCanvasUI; // X Button
    public event UI_CanvasActivation UIHandCanvas;
    
    // Event that controls the left grip for blur image in teleportation
    public delegate void mostionSicknesVignnette(bool i);
    public event mostionSicknesVignnette MotionSickVignetteTrigger;
    public event mostionSicknesVignnette movingOrRotating;

    // Event that controlls the Switch between layers in teleportation with the X Button

    // Event that controls the exit of the game with the Y Button in left controller
    public event Action yButton; // Exit

    public event Action aButton; // StoreInventory

    public event Action bButton; // Change to Teleport

    /////////////////// /////// Event Declairation \\\\\\\\\\\\\\\\\\\ \\\\\\\\\\\\\\\\\

    [Header("Buttons")]
    public InputActionProperty[] inputButtonAction;
    
    [Header("Controller Sticks")]
    public InputActionProperty leftGrip;
    private bool isArea;
    private bool isHittingPlaneCheck;

    public GameObject teleportHand;
    public GameObject grabHand;

    private bool isCanvasEnabled = false;
    private bool wasButtonXPressed = false;
    
    private void Awake()
    {
        mover = FindObjectOfType<ActionBasedContinuousMoveProvider>();
        turner = FindObjectOfType<ActionBasedContinuousTurnProvider>();

        // bButton += ChangeToTeleport;
    }
    void Update()
    {
        ButtonControllersInput();
        // MotionSwitchHandler();
    }
    public void ButtonControllersInput()
    {
        bool move = mover.leftHandMoveAction.action.IsInProgress();
        bool turn = turner.rightHandTurnAction.action.IsInProgress();

        if (move != LocomotionNotifier)
        {
            LocomotionNotifier = move;
            movingOrRotating?.Invoke(LocomotionNotifier);
        }

        foreach (var buttons in inputButtonAction)
        {
            bool buttonBPressed = inputButtonAction[0].action.IsPressed();
            bool buttonAPressed = inputButtonAction[1].action.IsPressed();                            
            bool buttonXPressed = inputButtonAction[2].action.IsPressed();
            bool buttonYPressed = inputButtonAction[3].action.IsPressed();
            bool rightStick =     inputButtonAction[4].action.inProgress;
            bool buttonSelectInProgress = inputButtonAction[5].action.IsPressed();

            ///////// B BUTTON \\\\\\\\
            
            if (buttonBPressed)
            {
                print("You Pressed " + inputButtonAction[1].action.name);

                if (bButton != null)
                {
                    bButton?.Invoke();
                }
            }
            ///////// A BUTTON \\\\\\\\\

            if (buttonAPressed)
            {
                if (aButton != null)
                {
                    aButton();
                }
            }

            ///////// X BUTTON \\\\\\\\\
            if (buttonXPressed && !wasButtonXPressed) // Detect button press transition
            {
                isCanvasEnabled = !isCanvasEnabled; // Toggle state
                activateCanvasUI?.Invoke(isCanvasEnabled);
            }

            wasButtonXPressed = buttonXPressed; // Update previous button state   
                   
            ///////// Y BUTTON \\\\\\\\\
            if (buttonYPressed)
            {
                print("You Pressed " + inputButtonAction[3].action.name);
                print("Exit");
                if(yButton != null)
                {
                    yButton();
                }  
            }
            ///////// RIGHT TRIGGER \\\\\\\\\    
                if (UIHandCanvas != null)
                { 
                    
                    // Check if the button is pressed and it wasn't pressed in the previous frame
                    if (buttonSelectInProgress)
                    {                     
                        // Call your delegate or method to update the UI state
                        // UIHandCanvas(enable);
                    }
                }

        }
    }

    public void MotionSwitchHandler()
    {
        isArea = FindObjectOfType<TeleportSwitch>().isInTeleportState;
        isHittingPlaneCheck = FindObjectOfType<TeleportSwitch>().IsHittingPlane;

        if(inputButtonAction[4].action.inProgress || isArea == true && isHittingPlaneCheck == true && leftGrip.action.IsPressed())
        {
            if(MotionSickVignetteTrigger != null)
            {
                MotionSickVignetteTrigger(true);
            }
            else
            {
                MotionSickVignetteTrigger(false);
            }
        }
        else
        {
            MotionSickVignetteTrigger(false);
        }
    }
}
