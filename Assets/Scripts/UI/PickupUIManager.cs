using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PickupUIManager : MonoBehaviour
{
    public static PickupUIManager Instance;

    [SerializeField] private GameObject promptPanel;   // This is the Panel GameObject
    [SerializeField] private TextMeshProUGUI promptText;          // Or use TextMeshProUGUI if needed

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        HidePrompt(); // Hide prompt at start
    }

    public void ShowPrompt(string message)
    {
        promptPanel.SetActive(true);
        promptText.text = message;
    }

    public void HidePrompt()
    {
        promptPanel.SetActive(false);
    }
}