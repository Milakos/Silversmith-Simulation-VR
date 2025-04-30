using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbagePool : MonoBehaviour
{
    public Dictionary<SO, Queue<GameObject>> grabables = new Dictionary<SO, Queue<GameObject>>();

    public Queue<GameObject> branches = new Queue<GameObject>();
    public Queue<GameObject> Ores = new Queue<GameObject>();
    public Dictionary<SO, GameObject> tools = new Dictionary<SO, GameObject>();

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float offset;
    public ButtonManager[] manager;
    private void Awake()
    {
        manager = FindObjectsOfType<ButtonManager>();

        foreach (ButtonManager mn in manager)
        {
            mn.SpawnRemovedObject += SpawnObject;
        }
    }
    private void SpawnObject(SO item)
    {
        if (item != null)
        {
            print(item.name + " is  Not Null");
            if (item.CanUseAsTool)
            {
                if (tools.ContainsKey(item))
                {
                    tools[item].gameObject.SetActive(true);
                    Debug.Log("Tool succesfully enabled");
                    tools[item].transform.position = FindTargetPosition();
                    tools.Remove(item);
                }
            }
            else
            {
                if (grabables.ContainsKey(item))
                {
                    if (item.type == Type.Branch)
                    {
                        if (branches.Count > 0)
                        {
                            GameObject ob = branches.Dequeue();
                            ob.SetActive(true);
                            ob.transform.position = FindTargetPosition();
                            print(ob.name + " has been spawned " + branches.Count + " branches");
                        }
                        // else
                        // {
                        //     Queue<GameObject> itemQueue = grabables[item];
                        //     
                        //     if (itemQueue.Count > 0)
                        //     {
                        //         GameObject ob = itemQueue.Dequeue();
                        //         ob.SetActive(true);                           
                        //         ob.transform.position = FindTargetPosition();
                        //         print(ob.name + " has been spawned branches out");
                        //     }
                        //     else
                        //     {
                        //         GameObject ob = itemQueue.Dequeue();
                        //         ob.SetActive(true);                           
                        //         ob.transform.position = FindTargetPosition();
                        //         grabables.Remove(item);
                        //         print($"has been spawned branches out {grabables.Count}");
                        //     }
                        // }
                    }
                    else if (item.type == Type.Silver)
                    {
                        if (Ores.Count != 0)
                        {
                            GameObject ob = Ores.Dequeue();
                            ob.SetActive(true);
                            // ob.GetComponent<Rigidbody>().isKinematic = true;
                            ob.transform.position = FindTargetPosition();
                        }
                        else
                        {
                            // Handle when the Ores queue is empty
                            Queue<GameObject> itemQueue = grabables[item];
                            if (itemQueue.Count > 0)
                            {
                                GameObject ob = itemQueue.Dequeue();
                                ob.SetActive(true);
                                // ob.GetComponent<Rigidbody>().isKinematic = true;
                                ob.transform.position = FindTargetPosition();
                            }
                            else
                            {
                                grabables.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        grabables.Remove(item);
                    }
                }
            }
        }
        else
        {
            print(item.name + " is Null");
        }


    }
    public Vector3 FindTargetPosition()
    {
        // Let's get a position infront of the player's camera
        return cameraTransform.position + (cameraTransform.forward * offset);
    }
    public void DestroyParent(SO Item, GameObject gameObject) 
    {
        if (Item.CanStored) 
        {
            if (!Item.CanUseAsTool)
            {
                if (Item.type == Type.Branch)
                {
                    if (!grabables.ContainsKey(Item))
                    {
                        grabables.Add(Item, branches);
                        branches.Enqueue(gameObject);
                        Debug.Log("Grababbles Add");
                    }
                    else
                    {
                        branches.Enqueue(gameObject);
                        Debug.Log("Branches Enqueue");
                    }
                }
                else if (Item.type == Type.Silver) 
                {
                    if (!grabables.ContainsKey(Item))
                    {
                        grabables.Add(Item, Ores);
                    }
                    else
                        Ores.Enqueue(gameObject);
                }
            }
            else 
            {
                tools.Add(Item, gameObject);
                Debug.Log("Tool succesfully inserted in tools");
            }
            
        }      
        // gameObject.SetActive(false);
    }
}
