using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSpawnManager : MonoBehaviour
{
    void Start()
    {
        var saveData = SaveManager.Instance.LastLoadedData;
        if (saveData == null) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = saveData.playerPosition;

        }
    }
}
