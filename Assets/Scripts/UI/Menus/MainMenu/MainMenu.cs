using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    [SerializeField] GameObject saveSlotUI;

    [Header("Main Menu Buttons")]
    [SerializeField] Button newGameButton;
    [SerializeField] Button loadGameButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button backButton;


    private void Awake()
    {
        newGameButton.onClick.AddListener(OnNewGameClicked);
        loadGameButton.onClick.AddListener(OnLoadGameClicked);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        quitButton.onClick.AddListener(QuitGame);
        backButton.onClick.AddListener(BackToMainMenu);
    }

    public void OnNewGameClicked()
    {
        SaveManager.Instance.SetGameMode(GameMode.NewGame);
        saveSlotUI.SetActive(true); // Opens slot UI
    }

    public void OnLoadGameClicked()
    {
        SaveManager.Instance.SetGameMode(GameMode.LoadGame);
        saveSlotUI.SetActive(true); // Same UI, different intent
    }

    private void OpenOptionsMenu()
    {
        mainMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }
    private void QuitGame()
    {
        Application.Quit();
    }

    private void BackToMainMenu()
    {
        optionsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
