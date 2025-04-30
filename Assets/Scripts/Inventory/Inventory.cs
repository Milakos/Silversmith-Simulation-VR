using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// An inventory class that is responsible for adding and removing items that uses want to store
    /// </summary>
    
    [SerializeField] public ButtonManager[] btnManager;

    private Dictionary<SO, int> items = new Dictionary<SO, int>();

    public List<InteractableManager> interactables = new List<InteractableManager>();

    private void Awake()
    {
        var inter = FindObjectsOfType<InteractableManager>().ToList();
        foreach (var item in inter)
        {
            interactables.Add(item);
        }

        foreach (InteractableManager manager in interactables)
        {
            manager.MountInventory += AddItem;           
        }
        foreach (ButtonManager button in btnManager) 
        {
           if(button != null)
           {
                button.RemoveItem += RemoveItem; 
           }                             
        }
    }
    /// <summary>
    /// A Method that called when the user chooses to store the item in the inventory.
    /// When the inventory slot is empty it adds the item based on its ID in the dictionary and increases the quantity to 1
    /// When an item is Added and already axists, increases only the quantity.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="ID"></param>
    public void AddItem(SO item, int ID) 
    {
        if (HasItem(item))
        {
            if (HasID(ID))
            {
                int existingID = items[item];
                int dictionaryItemIndex;                

                if (ID.GetHashCode() == existingID.GetHashCode())
                {
                    dictionaryItemIndex = items.Values.ToList().IndexOf(ID);

                    btnManager[dictionaryItemIndex].IncreaseQuantity(1);

                    print(dictionaryItemIndex);                              
                }
                else
                {
                    print("Interacted with an item with a different ID: " + ID + "" + item.GetHashCode());
                }
            }
            else
            {               
                print("NO ID Tracked");
            }
        }
        else
        {           
            items.Add(item, ID);            
            int dictionaryItemIndex = items.Values.ToList().IndexOf(ID);
            btnManager[dictionaryItemIndex].ChangeUIElement(item.Icon, item.Description, false, 1, item);
            print("Interacted with a new item with ID: " + ID);
        }
    }
    
    /// <summary>
    /// When this method is called removes the item given in the method parameter from the dictionary
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(SO item)
    {
        if (HasItem(item))
        {
            items.Remove(item);      
            Debug.Log("Succesfully Removed "  + item.name + "(s) from the inventory.");
        }
        else
        {
            // items.Remove(item);   
            Debug.Log(item.name + " not found in the inventory.");
        }
    }

    // Check if an item is present in the inventory
    public bool HasItem(SO item)
    {
        return items.ContainsKey(item);
    }
    /// <summary>
    /// A pure Function that that checks if the given value ID is the same as the parameter 
    /// and returns true or false for useage such as checking the inventory to update the image or quantity
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool HasID(int value) 
    {
        foreach (int existingID in items.Values) 
        {
            if (value == existingID) 
            {
                return true;
            }
        }
        return false;
    }
}
