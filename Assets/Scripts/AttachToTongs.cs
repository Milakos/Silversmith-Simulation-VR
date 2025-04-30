using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachToTongs : MonoBehaviour
{
    public Collider col;
    public Transform origin;
    bool attached = false;
    public CoolDownWater water;
    public ActionBasedController controller;
    private float trigger = 0f; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tongs"))
        {
            trigger = controller.activateAction.action.ReadValue<float>();

            if (trigger > 0 && !attached)
            {
                attached = true;
                this.gameObject.transform.SetParent(GameObject.Find("AttachMetal").transform);   
                this.gameObject.transform.localPosition = Vector3.zero;
                this.gameObject.transform.localRotation = Quaternion.identity;
                print ($"Attached {this.gameObject.name}");
            }
        }
    }
    
    public void TriggerExit()
    {
        if (water.isInState && attached)
        {
            // this.gameObject.transform.parent = null;
            this.gameObject.transform.SetParent(origin.gameObject.transform);
            this.gameObject.transform.position = origin.position;
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 40.0f, 0f);
            
            // col.isTrigger = false;
            attached = false;
            print ($"Dettached {this.gameObject.name}");
        }
    }
}
