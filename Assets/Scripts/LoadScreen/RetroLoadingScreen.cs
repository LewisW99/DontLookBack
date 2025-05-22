using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class RetroLoadingScreen : MonoBehaviour
{
    public float fakeLoadTime = 2.5f;
    public Image progressBar;
    private int targetIndex;

    [SerializeField] TextMeshProUGUI loadingText;
    private void Start()
    {
        // Grab the scene we want to load (set by SaveManager before this scene)
         targetIndex = PlayerPrefs.GetInt("NextSceneIndex", 1);
        StartCoroutine(FakeLoadRoutine());
        StartCoroutine(BlinkingDots(loadingText));
    }

    IEnumerator FakeLoadRoutine()
    {
        float start = Time.time;

        // Load scene in background, but don't activate yet
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetIndex);
        loadOp.allowSceneActivation = false;

        float timer = 0f;

        // Wait at least X seconds
        while (timer < fakeLoadTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fakeLoadTime);
            if (progressBar != null)
                progressBar.fillAmount = progress;

            yield return null;
        }

        // Now allow the scene to load in
        loadOp.allowSceneActivation = true;
    }

    IEnumerator BlinkingDots(TextMeshProUGUI text)
    {
        string baseText = "Now Loading";
        int dotCount = 0;

        while (true)
        {
            text.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4;
            yield return new WaitForSeconds(0.35f);
        }
    }

}
