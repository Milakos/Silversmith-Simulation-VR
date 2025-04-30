using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Image image;
    public TMP_Text textDescription;
    public TMP_Text textQuantity;
    
    public SO item;
    public int Quantity = 0;
    public bool hasItemInSlot;

    public delegate void RemoveItemEvent(SO item);
    // public event RemoveItemEvent Remove;
    public event RemoveItemEvent SpawnRemovedObject;
    public event RemoveItemEvent RemoveItem;
    private void Awake()
    {
        hasItemInSlot = false;
    }
    private void Update()
    {
        textQuantity.text = Quantity.ToString();
    }    
    public void ChangeUIElement(Sprite sprite, string text, bool slot, int quantity, SO item)
    {
        ChangeButtonImage(sprite);
        ChangeButtonDescription(text);
        IncreaseQuantity(quantity);
        ChangeSOItem(item);
        hasItemInSlot = slot;
    }
    private void OnTriggerEnter(Collider other) 
    {
        print($"Interact with {this.name} before");
        if(other.gameObject.CompareTag("Hand"))
        {
            if(item != null)
            {
                SpawnRemovedObject?.Invoke(item);
                print($"Interact with {this.name} Inside");
            }
            else
            {
                print($"Interact with {this.name} Outside");
            }

            DecreaseQuantity();
        }    
    }
/*    public void MinusChangeUIElement(Sprite sprite, string text, bool slot, SO item)
    {
        ChangeButtonImage(sprite);
        ChangeButtonDescription(text);
        DecreaseQuantity();
        ChangeSOItem(item);
        hasItemInSlot = slot;
    }*/
    public void ClearSlotData(Sprite sprite, string text, bool slot,  SO item) 
    {
        ChangeButtonImage(sprite);
        image.enabled = false;  
        ChangeButtonDescription(text);
        ChangeSOItem(item);
        hasItemInSlot = slot;
        /*Quantity = quantity;*/
    }
    public void ChangeButtonImage(Sprite sprite) 
    {
        image.sprite = sprite;
        image.enabled = true;  
    }
    public void ChangeButtonDescription(string text)
    {
        textDescription.text = text;
    }
    public void ChangeSOItem(SO newItem) 
    {
        item = newItem;
    }
    #region Quantity
    public int IncreaseQuantity(int quantity)
    {     
        Quantity++;
        return Quantity;
    }
    public void DecreaseQuantity() 
    {
        if (Quantity > 1)
        {
            Quantity--;
            print("Quantity decreased");
        }
        else if (Quantity == 1)
        {
            Quantity = 0;
            hasItemInSlot = false;  
            RemoveItem?.Invoke(item);
            /*FindObjectOfType<InventoryUIManager>().OnImageClicked();*/
            ClearSlotData(null, null, false, null);
            print("No more Items to withdraw");
        }       
    }
    #endregion Quantity

}
