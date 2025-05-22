using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PhotoPickupTrigger : MonoBehaviour
{
    public GameObject photoPrefab;
    public CanvasGroup flashPanel;
    public Vector3 localOffset = new Vector3(0f, -0.2f, 0.5f);
    public int nextSceneIndex = 2;

    public void PlayCinematic()
    {
        StartCoroutine(CinematicSequence());
    }

    IEnumerator CinematicSequence()
    {
        // Spawn the photo in front of the camera
        Transform cam = Camera.main.transform;
        GameObject floatingPhoto = Instantiate(photoPrefab, cam);
        floatingPhoto.transform.localPosition = localOffset;
        floatingPhoto.transform.localRotation = Quaternion.identity;

        // Float up slightly
        Vector3 endPos = floatingPhoto.transform.localPosition + new Vector3(0f, 0.1f, 0f);
        floatingPhoto.transform.DOLocalMove(endPos, 1f).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(1.5f);

        // Flash white
        flashPanel.gameObject.SetActive(true);
        flashPanel.alpha = 0f;
        flashPanel.DOFade(1f, 0.8f);

        yield return new WaitForSeconds(1f);

        // Go to loading screen
        PlayerPrefs.SetInt("NextSceneIndex", nextSceneIndex);
        SceneManager.LoadScene("LoadingScreen");
    }
}
