using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GenericObjectSurfaceType : MonoBehaviour
{
    public SurfaceType surfaceType;
    // public Collider col;
    // public LayerMask layerMask;

    // void Awake()
    // {
    //     col = GetComponent<Collider>();
    //     col.isTrigger = false;
    // }
    // private void OnCollisionEnter(Collision other) 
    // {
    //     if(other.collider.CompareTag("WoodHanlde"))
    //     {
    //         StaticObjectAudioEvent(other); 
    //         Debug.Log("Enter Surface");  
    //     }            
    // }
    // public void StaticObjectAudioEvent(Collision other)
    // {
    //     if(other.gameObject.CompareTag("WoodHanlde"))
    //     {
    //         other.gameObject.GetComponent<InteractableManager>().InteractableHandleAudio(surfaceType);
    //     }
    //     else
    //     {
    //         other.gameObject.GetComponent<InteractableManager>().InteractableAudio(surfaceType);
    //     }
    // }
}
