using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class SaveSlotSelector : MonoBehaviour
{
    public int slotNumber = 1;

    [Header("UI References")]
    public TextMeshProUGUI slotLabel;
    public TextMeshProUGUI slotScene;
    public TextMeshProUGUI slotTime;
    public GameObject confirmPopup;
    public TextMeshProUGUI confirmText;
    public Button confirmButton;
    public Button cancelButton;

    private SaveData slotData;

    private Button slotButton;

    private async void Start()
    {
        if (SaveManager.Instance.SaveExists(slotNumber))
        {
            string path = SaveManager.Instance.GetPathForSlot(slotNumber);
            string json = await System.IO.File.ReadAllTextAsync(path);
            slotData = JsonUtility.FromJson<SaveData>(json);

            slotLabel.text = $"Slot {slotNumber}";
            slotScene.text = $"Scene: {slotData.sceneName}";
            slotTime.text = $"Last Played: {slotData.timestamp}\nTime: {FormatTime(slotData.totalPlayTimeSeconds)}";
        }
        else
        {
            slotLabel.text = $"Slot {slotNumber}";
            slotScene.text = "Empty Slot";
            slotTime.text = "";
        }
        slotButton = GetComponent<Button>();
        slotButton.onClick.AddListener(OnSlotClicked);

    }

    public async void OnSlotClicked()
    {
        SaveManager.Instance.SetActiveSlot(slotNumber);
        var mode = SaveManager.Instance.CurrentGameMode;

        bool hasSave = slotData != null;

        if (mode == GameMode.NewGame)
        {
            if (hasSave)
            {
                confirmText.text = "This slot already has data. Overwrite it?";
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(() =>
                {
                    SaveManager.Instance.DeleteSlot(slotNumber);

                    // Load Level 1 via LoadingScreen
                    PlayerPrefs.SetInt("NextSceneIndex", 1); // Build Index of Level 1
                    SceneManager.LoadScene("LoadingScreen");
                });
            }
            else
            {
                PlayerPrefs.SetInt("NextSceneIndex", 1); // Build Index of Level 1
                SceneManager.LoadScene("LoadingScreen");
                return;
            }
        }
        else if (mode == GameMode.LoadGame)
        {
            if (hasSave)
            {
                confirmText.text = "Load saved game from this slot?";
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(async () =>
                {
                    var data = await SaveManager.Instance.LoadSaveAsync();
                    if (data != null)
                    {
                        PlayerPrefs.SetInt("NextSceneIndex", data.sceneBuildIndex);
                        SceneManager.LoadScene("LoadingScreen");
                    }
                });
            }
            else
            {
                confirmText.text = "No saved data in this slot.";
                confirmButton.gameObject.SetActive(false);
            }
        }

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            confirmPopup.SetActive(false);
            confirmButton.gameObject.SetActive(true);
        });

        confirmPopup.SetActive(true);
    }



    private string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.FloorToInt(seconds);
        int hrs = totalSeconds / 3600;
        int mins = (totalSeconds % 3600) / 60;
        int secs = totalSeconds % 60;
        return $"{hrs:D2}:{mins:D2}:{secs:D2}";
    }
}
