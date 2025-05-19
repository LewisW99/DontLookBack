using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private string pickupName = "Item";

    private int defaultLayer;
    private int outlineLayer;

    
    protected bool isPlayerInRange = false;
    protected GameObject player;

    public abstract void OnPickup(GameObject player);


    private void Start()
    {
        defaultLayer = LayerMask.NameToLayer("Default");
        outlineLayer = LayerMask.NameToLayer("Outline");
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OnPickup(player);
            Destroy(gameObject);
            HidePickupUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.gameObject;
            ShowPickupUI();
            gameObject.layer = outlineLayer;
            ToggleOutline(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
            HidePickupUI();
            gameObject.layer = defaultLayer;
            ToggleOutline(false);
        }
    }

    private void ShowPickupUI()
    {
        PickupUIManager.Instance.ShowPrompt($"Press 'E' to pick up {pickupName}");
    }

    private void HidePickupUI()
    {
        PickupUIManager.Instance.HidePrompt();
    }

    private void ToggleOutline(bool active)
    {
        
    }
}