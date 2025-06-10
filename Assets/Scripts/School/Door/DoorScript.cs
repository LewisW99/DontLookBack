using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool isUnlocked = false;

    public void UnlockPadlock()
    {
        Debug.Log("Custom padlocked door unlocked!");
        isUnlocked = true;

        // Add your specific open behavior here:
        Open();
    }

    private void Open()
    {
        // Animate, rotate, fade, whatever.
        transform.Rotate(Vector3.up * 90f);
    }
}