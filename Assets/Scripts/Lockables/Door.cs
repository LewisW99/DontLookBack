using UnityEngine;

public class Door : Lockable, IInteractable
{
    public float interactRange = 3f;
    private GameObject player;

    private void Update()
    {
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= interactRange)
            {
                InteractionUIManager.Instance.ShowPrompt("Press 'E' to open door");
            }
            else
            {
                InteractionUIManager.Instance.HidePrompt();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            InteractionUIManager.Instance.HidePrompt();
        }
    }

    public void Interact(GameObject interactor)
    {
        PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            TryUnlock(inventory);
            InteractionUIManager.Instance.HidePrompt();
        }
    }

    protected override void Unlock()
    {
        Debug.Log("Door unlocked and opened!");
        gameObject.SetActive(false);
        // Play animation etc.
    }
}