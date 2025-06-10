using UnityEngine;

public class Padlock : MonoBehaviour
{
    public DoorScript doorScript; // Assign any script that has an UnlockPadlock() method
    public GameObject visualPadlock;

    public void Break()
    {
        Debug.Log("Padlock broken!");

        if (visualPadlock != null)
            Destroy(visualPadlock);

        // Dynamically call UnlockPadlock() if it exists
        if (doorScript != null)
        {
            var method = doorScript.GetType().GetMethod("UnlockPadlock");
            if (method != null)
            {
                method.Invoke(doorScript, null);
            }
            else
            {
                Debug.LogWarning("Door script does not have UnlockPadlock()");
            }
        }

        Destroy(gameObject); // Optional
    }
}