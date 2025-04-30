using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeRecticle : MonoBehaviour
{
    XRInteractorLineVisual lineVisual;
    // XRInteractorLineVisual SecondlineVisual;
    [SerializeField] Dictionary< string, GameObject> prefab = new Dictionary<string, GameObject>();
    [SerializeField] GameObject[] prefabs;


    private void Awake() 
    {
        lineVisual = GetComponent<XRInteractorLineVisual>();  
    }
    private void Start() 
    {
        foreach (GameObject obj in prefabs)
        {
            GameObject newObj = Instantiate(obj, Vector3.zero, Quaternion.identity);
            newObj.name = obj.name;    
            prefab.Add(obj.name, newObj);
            newObj.SetActive(false);
        }
    }
    private void OnEnable() 
    {
        FindObjectOfType<TeleportSwitch>().changeRecticlePrefabObject += ChangeRecticlePrefab;
    }
    public void ChangeRecticlePrefab(bool objFlag)
    {   

        if(objFlag == true)
        {
            // UpdatePrefabReticleToTeleport(lineVisual);
            // InitializePrefab(prefabs[0].name);   
            print("OBJECT TRUE");
        }
        else
        {
            UpdatePrefabReticleToInteract(lineVisual);
            // InitializePrefab(prefabs[1].name); 
            print("OBJECT FALSE");
        }   
    }

    public void UpdatePrefabReticleToTeleport(XRInteractorLineVisual lineVisual)
    {
        InitializePrefab(prefabs[0].name);
        prefabs[1].SetActive(false);
    }

    public void UpdatePrefabReticleToInteract(XRInteractorLineVisual lineVisual)
    {
        InitializePrefab(prefabs[1].name);
        prefabs[0].SetActive(false);
    }

    public void InitializePrefab(string name)
    {
        if (prefabs != null)
        {
            lineVisual.reticle = prefab[name];
            lineVisual.reticle.SetActive(true);
            
            foreach(GameObject go in prefab.Values)
            {
                if(go.name != name)
                {
                    go.SetActive(false);
                }
            }
        }

  
    }
}
