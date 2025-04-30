using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FMODUnity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;


[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableManager : MonoBehaviour, IItemInventory
{

    [Header("Item Properties")] [SerializeField]
    [Tooltip("Select the type of this tool")]
    public SO Item; // The Scripatble object item that this script will inherit properties
    public ToolType toolTypeHandle;
    public GameObject ItemsSocket;
    [Space(10)]
    public Collider parentCollider;
    public Collider childCollider;

    [Header("Interactable Reference")] [SerializeField]
    private XRGrabInteractable baseInteractable; // Reference of the XRGrabInteracyable Component
    [Space(10)]
    [Header("Hand Interactors")]
    public XRBaseInteractor[] baseInteractors; // The initialization of the hands through Inspector,
                                               // for a reason only works when access modifier is public 

    public List<GameObject> hands = new List<GameObject>(); // List of gameobjects that will be added
    [Header("Layer Identifier Usability")] [Space(10)] [Tooltip("Choose the condition of the object after is attached to its socket")]
    [SerializeField] private InteractionLayerMask mask; // Layer Mask that identifies the object state such as "is Placed"

    /// <summary>
    /// Const Variables that will not change
    /// </summary>
    private const bool Deactivate = false; // A Const bool that always have to be False for hand deactivation
    private const int LayerInteractable = 10;
    private const int LayerHands = 12;
    [SerializeField] public string AnimationNameHash = "";

    /// <summary>
    /// 
    /// </summary>
    private Animator anim; // Reference of Animator Componenet
    private Rigidbody rb; // Reference of Rigidbody component
    
    private ButtonActionsController controller;
    public delegate void MountInInventory(SO item, int id);
    public event MountInInventory MountInventory;
    Inventory inventory;
    InteractableManager instance;
    [Inject] private GarbagePool garbage;
    bool isSelected = false;

    public Queue<GameObject> objbranch = new Queue<GameObject>();
    public Queue<GameObject> objores = new Queue<GameObject>();
    // public GameObject[] playerHands = new GameObject[2];
    [Inject] protected IAudioManager audioManager;
    //Store the previous velocity so we can compute acceleration.
    private Vector3 previousVelocity;
    public bool doubleHande = false;

    private List<ButtonManager> btn = new List<ButtonManager>();
    private void Awake()
    {
        instance = this;
        //
        if (Item.type == Type.Branch)
            objbranch.Enqueue(instance.gameObject);
        else if(Item.type == Type.Silver)
            objores.Enqueue(instance.gameObject);        
        //
        inventory = FindObjectOfType<Inventory>();       
        controller = FindObjectOfType<ButtonActionsController>();
        // garbage = FindObjectOfType<GarbagePool>();
        baseInteractable = GetComponent<XRGrabInteractable>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (baseInteractors == null) { return; }
        baseInteractors = FindObjectsOfType<XRBaseInteractor>();
        
    }
    
    private void OnEnable()
    {      
        baseInteractable.selectEntered.AddListener(OnSelectEnter);
        baseInteractable.selectExited.AddListener(OnSelectExit);    
        
        btn = FindObjectsOfType<ButtonManager>().ToList();
        
        foreach (var item in btn)
        {
            item.SpawnRemovedObject += (item) => RemoveFromInventory();
        }
    }
    private void Start()
    {
        previousVelocity = rb.velocity;
        DeactivateHands();
        CollidesWithForge(false);
        isSelected = false;
    }
    private void OnDisable()
    {
        inventory.interactables.Remove(instance);
        controller.aButton -= StoreInInventory;
        controller.movingOrRotating -= MoveOrRotate;
        baseInteractable.selectEntered.RemoveListener(OnSelectEnter);
        baseInteractable.selectExited.RemoveListener(OnSelectExit);
        isSelected = false;
    }
    // Method that Triggers all the functionality when an interactor grabs this interactable

    private void OnSelectEnter(SelectEnterEventArgs EnterEvents)
    {
        
        anim.applyRootMotion = true;
        
        if(ItemsSocket != null)
            ItemsSocket.SetActive(true);
        HandSelectChoose(true);
        StopEmissionEffectAnimation(true);
        
        controller.aButton += StoreInInventory;
        controller.movingOrRotating += MoveOrRotate;
        isSelected = true;

    }

    private void MoveOrRotate(bool moving)
    {
        if(moving == true)
        {
            baseInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
        }
        else
        {
            baseInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        }
    }

    // Method that Triggers all the functionality when an interactor release this interactable
    private void OnSelectExit(SelectExitEventArgs ExitEvents)
    {
        if (CollidesWithForge(false)) 
        {
            if (ItemsSocket != null)
                ItemsSocket.SetActive(false);
        }

        HandSelectChoose(false);
        StopEmissionEffectAnimation(false);
        anim.applyRootMotion = false;
        DeactivateHands();
        ApplyPhysicsToInteractable();
        controller.aButton -= StoreInInventory;
        controller.movingOrRotating -= MoveOrRotate;
        isSelected = false;

    }

    // Function that checks which of the two interactors aka hands interact with this gameobject 
    public void HandSelectChoose(bool handActivation)
    {
        if(doubleHande == false)
        {
            if (baseInteractors != null) 
            {
                if (baseInteractors[0].IsSelecting(baseInteractable)) // right
                {                
                    hands[0].SetActive(handActivation);               
                }
                else if (baseInteractors[1].IsSelecting(baseInteractable)) // left
                {
                    hands[1].SetActive(handActivation);
                }
            } 
        } 
        else
        {
            if (baseInteractors != null) 
            {
                if (baseInteractors[0].IsSelecting(baseInteractable))
                {                
                    hands[0].SetActive(handActivation);               
                } 
                if (baseInteractors[1].IsSelecting(baseInteractable))
                {                
                    hands[1].SetActive(handActivation);               
                } 
            }
        }
      
    }

    // A Function that cannot let the player interact with a placed object in the scene
    //through Socket GameObject
    public void OnSocketDetach()
    {
        baseInteractable.interactionLayers = mask;
    }
    // A Function for triggering the emission animation componenet through grabbing from player
    public void StopEmissionEffectAnimation(bool grabbingBool)
    {
        anim.SetBool("isGrabbed", grabbingBool);
    }

    // A Funcion that only Set Active to false both attached hand gameobjects
    public void DeactivateHands()
    {
        foreach (GameObject hand in hands)
        {
            if (hand != null) 
            {
                hand.SetActive(Deactivate);
            }          
        }
    }
    /// <summary>
    /// When the player releases the interactable object thiw method applies force of gravity for a realistic 
    /// throw and Ignores the Collision with the hands to prevent from bouncing effects
    /// </summary>
    private void ApplyPhysicsToInteractable()
    {
        rb.isKinematic = false;
    }

    /// <summary>
    /// Function that Destroys the gamobject that this component is attached to through animation events
    /// </summary>
    public void MoveToGarbagePoolAndSetActive() 
    {
        garbage.DestroyParent(Item, gameObject);     
        this.gameObject.SetActive(false);
    }

    public bool CanUsedAsATool()
    {
        return Item.CanUseAsTool;
    }
    public bool CanBeStored()
    {
        return Item.CanStored;
    }
    public void Use()
    {
        if (!CanUsedAsATool()) { return; }

        if (CanUsedAsATool() == true) 
        {
            print("This is a Tool");
        }
    }

    public void StoreInInventory()
    {
        if (!CanBeStored()) { return; }
        
        if (CanBeStored() == true)
        {
            DeactivateHands();
            FindObjectOfType<HandUI>().anim.SetBool("HandUIActivated", true);
            anim.Play(AnimationNameHash);
            //TODO add some functionality that puts the data in the inventory list
            if (MountInventory != null) 
            {               
                MountInventory?.Invoke(Item, Item.ID);
                controller.aButton -= StoreInInventory;
            }
        }
        else
        {           
            FindObjectOfType<HandUI>().anim.SetBool("HandUIActivated", true);
        }
        if (ItemsSocket != null)
            ItemsSocket.SetActive(false);
    }
    public void RemoveFromInventory()
    {
        if(rb!=null)
        {
            rb.isKinematic = true;
            Debug.Log("Remove from inventory");
        }
    }

    public void DestoryOnSocket() 
    {
        Destroy(gameObject);
        Destroy(ItemsSocket);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Forge"))
        {
            CollidesWithForge(true);
        }
    }

    public bool CollidesWithForge(bool collides) 
    {
        return collides;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(isSelected)
        {
            // Vector3 impulse = collision.impulse;
            // // Optionally, you can compute the magnitude to get a scalar force measure.
            // float forceMagnitude = impulse.magnitude;

            // // relativeVelocity is the velocity of the incoming object relative to this object.       
            // Vector3 relativeVel = collision.relativeVelocity;
            // float relativeSpeed = relativeVel.magnitude;
            // float estimatedForce = rb.mass * relativeSpeed; // This is a rough estimation.

            // Debug.Log($"After-Swipe Collision impulse magnitude: {forceMagnitude} and estimated Force is {estimatedForce} ");

            if (collision.collider.CompareTag("Furniture"))
            {
                // if (childCollider == null)

                foreach (ContactPoint contact in collision.contacts)
                {
                    // contact.thisCollider is the collider attached to this object (or its children)
                    Collider collidedChild = contact.thisCollider;

                    if (collidedChild == parentCollider)
                    {
                        collision.collider.TryGetComponent<GenericObjectSurfaceType>(out GenericObjectSurfaceType surface);
                        InteractableAudio(surface.surfaceType, contact);              
                    }
                    else if (collidedChild == null && collidedChild == childCollider)
                    {
                        collision.collider.TryGetComponent<GenericObjectSurfaceType>(out GenericObjectSurfaceType surface);
                        InteractableHandleAudio(surface.surfaceType);             
                    }
                }
            }
        }      
        else
        {
            if (collision.collider.CompareTag("Furniture"))
            {
                if(childCollider == null) return;

                foreach (ContactPoint contact in collision.contacts)
                {
                    // contact.thisCollider is the collider attached to this object (or its children)
                    Collider collidedChild = contact.thisCollider;
                    
                    
                    if (collidedChild == parentCollider)
                    {
                        collision.collider.TryGetComponent<GenericObjectSurfaceType>(out GenericObjectSurfaceType surface);
                        if(surface.surfaceType == SurfaceType.Floor)
                        {
                            InteractableAudio(surface.surfaceType, contact);  
                        }
                                    
                    }
                    else if (collidedChild == null && collidedChild == childCollider)
                    {
                        collision.collider.TryGetComponent<GenericObjectSurfaceType>(out GenericObjectSurfaceType surface);
                        if(surface.surfaceType == SurfaceType.Floor)
                        {
                            InteractableHandleAudio(surface.surfaceType);  
                        }              
                    }
                } 
            }
        }
    }
    // private void FixedUpdate() 
    // {
    //     Vector3 currentVelocity;
    //     float deltaTime = Time.fixedDeltaTime;
        
    //     // Get the current velocity from the Rigidbody.
    //     currentVelocity = rb.velocity;         
    //     // Compute acceleration (change in velocity over time).
    //     Vector3 acceleration = (currentVelocity - previousVelocity) / deltaTime;
        
    //     // Compute force using F = m * a. Here we use the Rigidbody's mass if available,
    //     // or assume a default mass of 1 if not.
    //     float mass = (rb != null) ? rb.mass : 1f;
    //     Vector3 force = mass * acceleration;
        
    //     // Print the values to the console.
    //     Debug.LogFormat("Swipe Velocity: {0:F2} m/s", currentVelocity.magnitude);
    //     Debug.LogFormat("Swipe Acceleration: {0:F2} m/s²", acceleration.magnitude);
    //     Debug.LogFormat("Swipe Force: {0:F2} N", force.magnitude);
        
    //     // Update previous velocity for the next frame.
    //     previousVelocity = currentVelocity;
    // }
    public void InteractableAudio(SurfaceType surfaceType, ContactPoint contact)
    {
        // Look up the audio event for this tool–surface combination.
        EventReference evt = audioManager.GetToolSurfaceAudioEvent(Item.toolType, surfaceType);
        if (!evt.IsNull)
        {
            audioManager.PlayOneShot(evt, contact.point);
        }
    }
    public void InteractableHandleAudio(SurfaceType surfaceType)
    {
        // Look up the audio event for this tool–surface combination.
        EventReference evt = audioManager.GetToolSurfaceAudioEvent(toolTypeHandle, surfaceType);
        if (!evt.IsNull)
        {
            audioManager.PlayOneShot(evt, this.transform.position);
        }
    }
}
