using UnityEngine;

[CreateAssetMenu (fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class SO : ScriptableObject
{
    [Header("Item Identity")]
    [Space(10)]
    public int ID;
    public string ItemName;
    public Type type;
    public ToolType toolType;
    public GameObject prefabGrabbable;
    public GameObject prefabSocket;
    
    public bool CanStored;
    public bool CanUseAsTool;

    [Header("Properties")]
    [Space(10)]
    public Sprite Icon;
    [Space(5)]
    [TextArea(0,10)]
    public string Description;
}
public enum ToolType
{
    None,
    Axe,
    Hammer,
    Pickaxe,
    Shaw,
    Screw,
    Cauldron,
    Tongs
    
    // Add additional tool types as needed
}
public enum Type 
{
    Branch, Charchoal, Silver, Tool
}
