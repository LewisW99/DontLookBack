using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    None,
    NewGame,
    LoadGame
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string saveFolder;
    private int activeSlot = 1;

    private float sessionStartTime;

    public GameMode CurrentGameMode { get; private set; } = GameMode.None;

    public void SetGameMode(GameMode mode)
    {
        CurrentGameMode = mode;
    }

    public SaveData LastLoadedData { get; private set; }




    private void Start()
    {
        sessionStartTime = Time.realtimeSinceStartup;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFolder = Application.persistentDataPath;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActiveSlot(int slot)
    {
        activeSlot = Mathf.Clamp(slot, 1, 3);
        Debug.Log($"[Save] Slot {activeSlot} selected.");
    }

    private string GetSlotPath() => Path.Combine(saveFolder, $"save_slot_{activeSlot}.json");

    public async Task AutoSaveAsync(Vector3 playerPosition)
    {
        float elapsed = Time.realtimeSinceStartup - sessionStartTime;

        SaveData data = new SaveData
        {
            sceneBuildIndex = SceneManager.GetActiveScene().buildIndex,
            sceneName = SceneManager.GetActiveScene().name,
            playerPosition = playerPosition,
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            totalPlayTimeSeconds = elapsed
        };

        string json = JsonUtility.ToJson(data);
        string path = GetSlotPath();

        using StreamWriter writer = new StreamWriter(path, false);
        await writer.WriteAsync(json);

        Debug.Log($"[Save] Autosaved to {path}");
    }

    public async Task<SaveData> LoadSaveAsync()
    {
        string path = GetSlotPath();
        if (!File.Exists(path)) return null;

        string json = await File.ReadAllTextAsync(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        LastLoadedData = data; // store it for use after scene load
        return data;
    }

    public bool SaveExists(int slot)
    {
        return File.Exists(Path.Combine(saveFolder, $"save_slot_{slot}.json"));
    }

    public void DeleteSlot(int slot)
    {
        string path = Path.Combine(saveFolder, $"save_slot_{slot}.json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[Save] Deleted save slot {slot}");
        }
    }

    public string GetPathForSlot(int slot)
    {
        slot = Mathf.Clamp(slot, 1, 3);
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slot}.json");
    }
}
