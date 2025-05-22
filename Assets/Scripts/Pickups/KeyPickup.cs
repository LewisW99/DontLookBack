using UnityEngine;

public class KeyPickup : Pickup
{
    [SerializeField] private KeyData keyData;

    public override void OnPickup(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null && keyData != null)
        {
            inventory.AddItem(keyData);
            Debug.Log($"Picked up key: {keyData}");
        }
        else
        {
            Debug.LogWarning("PlayerInventory missing or keyData not assigned!");
        }
    }
}