using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class CustomXRFilter : MonoBehaviour, IXRSelectFilter
{
    public bool canProcess { get; }
    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        bool canSelect = true;
        // Do additional validation checks on the interactor and interactable.
        print("XR Select");
        return interactable.transform.CompareTag("IngotMetalSheet");
    }
}
