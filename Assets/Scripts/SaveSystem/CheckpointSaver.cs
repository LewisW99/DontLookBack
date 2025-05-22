using UnityEngine;

public class CheckpointSaver : MonoBehaviour
{
    private bool triggered;

    private async void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            

            await SaveManager.Instance.AutoSaveAsync(other.transform.position);

            // Optional: Show “Saving…” screen, SFX, or flash
        }
    }
}
