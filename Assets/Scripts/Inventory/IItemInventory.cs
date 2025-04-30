public interface IItemInventory
{
    /// <summary>
    /// An interface that Grabable Interactors will inherit
    /// </summary>
    /// <returns></returns>
    public abstract bool CanUsedAsATool();
    public abstract bool CanBeStored();
    public abstract void Use();
    public abstract void StoreInInventory();
    public abstract void RemoveFromInventory();
    
}
