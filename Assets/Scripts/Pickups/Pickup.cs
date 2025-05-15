// Base class
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public abstract void OnPickup(GameObject player);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup(other.gameObject);
            Destroy(gameObject);
        }
    }
}
