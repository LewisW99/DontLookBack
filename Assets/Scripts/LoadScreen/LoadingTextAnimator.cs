using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
public class LoadingTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public float blinkSpeed = 0.0f;
    private string baseText = "Now Loading";

    void Start()
    {
        StartCoroutine(AnimateLoadingText());
    }

    IEnumerator AnimateLoadingText()
    {
        int dotCount = 0;
        bool visible = true;

        while (true)
        {
            string dots = new string('.', dotCount);
            loadingText.text = visible ? $"{baseText}{dots}" : "";

            dotCount = (dotCount + 1) % 4;
            visible = !visible;

            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
