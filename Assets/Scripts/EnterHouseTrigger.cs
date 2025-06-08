using System;
using UnityEngine;

public class EnterHouseTrigger : MonoBehaviour
{
    public bool canEnterHouse = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press E To Open House");

            canEnterHouse = true;
        }
    }

   
}
