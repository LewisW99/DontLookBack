using UnityEngine;

public class PhotoPickup : Pickup
{
    [SerializeField] private PhotoData photoData;
    [SerializeField] private GameObject pickupTrigger;

    public override void OnPickup(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventory>();
        if (inventory && photoData)
        {
            inventory.AddItem(photoData);
        }

        pickupTrigger?.GetComponent<PhotoPickupTrigger>()?.PlayCinematic();
    }
}
