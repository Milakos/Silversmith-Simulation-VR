using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    
    [System.Serializable]
    public class ButtonData  
    {
        public Image image;
        public TMP_Text textDescription;
        public SO item;
        public bool hasSlot;
        public int quantity;
    }

    public ButtonManager[] buttons;

    public List<Image> imageComponents;
    public List<TMP_Text> textComponents;
    public List<ButtonManager> itemComponents;
    public List<SO> items;
    public List<bool> hasSlotComponents;
    public List<int> quantityComponents;

    private Dictionary<int, ButtonData> imageDataDictionary = new Dictionary<int, ButtonData>();
    /*private int currentIndex = 0;*/
/*
    private void Start()
    {
        for (int i = 0; i < imageComponents.Count; i++)
        {
            items.Add(itemComponents[i].item);

            ButtonData btnData = new ButtonData
            {
                image = buttons[i].image,
                textDescription = buttons[i].textDescription,
                item = itemComponents[i].item,
                hasSlot = buttons[i].hasItemInSlot,
                quantity = buttons[i].Quantity
            };

            imageDataDictionary.Add(i, btnData);
        }
    }
    public void FillNextImageAndText() 
    {
        if (imageDataDictionary.TryGetValue(currentIndex, out ButtonData data))
        {
            imageComponents[currentIndex].sprite = data.image.sprite;
            textComponents[currentIndex].text = data.textDescription.text;
            items[currentIndex] = data.item;
            hasSlotComponents[currentIndex] = data.hasSlot;
            quantityComponents[currentIndex] = data.quantity;
            currentIndex++;
        }
        else
        {
            Debug.LogWarning("No more images and texts to fill!");
        }

    }
    public void OnImageClicked()
    {
            currentIndex++;

            // Shift subsequent images and texts to fill the empty slot
            for (int i = 0; i < imageComponents.Count -1 ; i++)
            {
                *//*imageDataDictionary[i] = imageDataDictionary[i + 1];*//*
                imageComponents[i].sprite = imageComponents[i + 1].sprite;
                textComponents[i].text = textComponents[i + 1].text;
            items[i] = items[i + 1];
            hasSlotComponents[i] = hasSlotComponents[i + 1];
            quantityComponents[i] = quantityComponents[i + 1];
        }

            // Clear the last image and text components
        int lastIndex = imageComponents.Count -1;

        imageComponents[lastIndex].sprite = null;
        textComponents[lastIndex].text = string.Empty;
        items[lastIndex] = null;
        hasSlotComponents[lastIndex] = false;
        quantityComponents[lastIndex] = 0;

        // Remove the last element from the dictionary since it's duplicated now
        imageDataDictionary.Remove(lastIndex);

        // Decrement the current index if it was beyond the last filled index
        currentIndex = Mathf.Min(currentIndex, lastIndex);

    }*/
}
