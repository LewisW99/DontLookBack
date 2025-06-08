using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 3f;
    private Camera playerCamera;

    [SerializeField] int nextSceneIndex;
    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(gameObject);
                }
            }

            if (FindFirstObjectByType<EnterHouseTrigger>().canEnterHouse)
            {
                // Go to loading screen
                PlayerPrefs.SetInt("NextSceneIndex", nextSceneIndex);
                SceneManager.LoadScene("LoadingScreen");
            }
        }
    }
}