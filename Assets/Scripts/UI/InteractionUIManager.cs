using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance;

    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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