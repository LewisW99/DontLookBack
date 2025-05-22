using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]

    [Header("UI GO References")]
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject optionsMenuUI;

    [Header("Pause Menu Buttons")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;

    public bool isPaused;

    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                
                OpenPauseMenu();
            }

        }
    }

    public void OpenPauseMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; // Unlock the cursor
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);

    }

    private void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);

    }
    private void OpenOptionsMenu()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);

    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
