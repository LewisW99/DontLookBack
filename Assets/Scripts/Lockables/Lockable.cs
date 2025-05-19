using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interactor);
}

public abstract class Lockable : MonoBehaviour
{
    [SerializeField] private KeyData requiredKey;

    public virtual bool TryUnlock(PlayerInventory inventory)
    {
        if (inventory.HasKey(requiredKey))
        {
            Unlock();
            return true;
        }

        Debug.Log("Missing key: " + requiredKey.displayName);
        return false;
    }

    protected abstract void Unlock();
}