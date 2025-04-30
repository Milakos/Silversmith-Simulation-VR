using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class HandUI : MonoBehaviour
{

    XRGrabInteractable[] grabItem;
    private List<XRGrabInteractable> items = new List<XRGrabInteractable>();
    ButtonActionsController controller;
    FollowVision followVision;
    [HideInInspector] public Animator anim;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject canvas;
    [SerializeField] private float offset;

    bool isCentered = false;

    [SerializeField] private Image image;
    private TMP_Text text;

    private void Awake()
    {
        controller = FindObjectOfType<ButtonActionsController>();
        grabItem = FindObjectsOfType<XRGrabInteractable>();
        anim = GetComponentInChildren<Animator>();
        followVision = GetComponentInChildren<FollowVision>();

        text = GetComponentInChildren<TMP_Text>();
    }
    private void OnEnable()
    {
        /*controller.UIHandCanvas += ActivateHandUI;   */   
    }

    private void OnDisable()
    {
        /*controller.UIHandCanvas -= ActivateHandUI;*/
        StopCoroutine(CloseUITime());
    }
 
    #region FollowVision
    private Vector3 FindTargetPosition()
    {
        // Let's get a position infront of the player's camera
        return cameraTransform.position + (cameraTransform.forward * offset);
    }
    private void MoveTowards(Vector3 targetPosition)
    {
        // Instead of a tween, that would need to be constantly restarted
        canvas.transform.position += (targetPosition - canvas.transform.position) * 0.025f;
    }
    private bool ReachedPosition(Vector3 targetPosition)
    {
        // Simple distance check, can be replaced if you wish
        return Vector3.Distance(targetPosition, canvas.transform.position) < 0.1f;
    }
    private Quaternion InitRotation() 
    {
       return canvas.transform.rotation = cameraTransform.transform.rotation.normalized;
    }
    private void CenteredVision()
    {
        if (!followVision) { return; }
        isCentered = followVision.centered;


        if (!isCentered)
        {
            //Initialize Rotation
            InitRotation();
            // Find the position we need to be at
            Vector3 targetPosition = FindTargetPosition();

            // Move just a little bit at a time
            MoveTowards(targetPosition);

            // If we've reached the position, don't do anymore work
            if (ReachedPosition(targetPosition))
                isCentered = true;
        }
    }
    #endregion FollowVision

    private void ActivateHandUI(bool setActive)
    {
        if (!isGrabbedNotifier()) 
        {
            StartCoroutine(CloseUITime());
        }
        if (isGrabbedNotifier() == true)
        {
            CenteredVision();
            canvas.gameObject.SetActive(setActive);
        }
    }
    public bool isGrabbedNotifier()
    {   
        foreach (XRGrabInteractable interactable in grabItem) 
        {
            if (interactable.isSelected) 
            {
                image.sprite = interactable.gameObject.GetComponent<InteractableManager>().Item.Icon;
                return true;
            }
        }
        return false;   
    }

    public IEnumerator CloseUITime() 
    {
        if(anim != null)
            anim.SetBool("HandUIActivated", true);
        yield return new WaitForSeconds(2.0f);
        canvas.gameObject.SetActive(false);
    }

}
