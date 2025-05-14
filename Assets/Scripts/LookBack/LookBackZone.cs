using UnityEngine;

public class LookBackZone : MonoBehaviour
{
    private bool qteTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (qteTriggered || !other.CompareTag("Player")) return;

        qteTriggered = true;
        if (!LookBackQTEManager.Instance.IsQTEActive)
        {
            LookBackQTEManager.Instance.StartQTE();
        }
    }
}