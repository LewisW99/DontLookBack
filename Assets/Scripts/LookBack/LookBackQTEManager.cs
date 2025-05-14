using System.Collections;
using UnityEngine;

public class LookBackQTEManager : MonoBehaviour
{
    public static LookBackQTEManager Instance { get; private set; }

    [SerializeField] private CanvasGroup qteCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("QTE Settings")]
    public float validLookBackTime = 2.0f;
    public float gracePeriod = 1.0f;
    public float lookDuration = 1.0f;
    public float lookRotationSpeed = 5f;
    public float lookBackThreshold = 160f;

    [SerializeField] private GameObject qteOverlay;

    private Transform cameraTransform;
    private Quaternion originalRotation;
    private Quaternion lookBackRotation;
    private Coroutine fadeRoutine;

    private float qteStartTime;
    private bool qteActive = false;
    private bool isLookingBack = false;

    public bool IsQTEActive => qteActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        cameraTransform = Camera.main?.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Camera not found! QTE system requires a main camera.");
        }
        
        if (qteCanvasGroup != null)
        {
            qteCanvasGroup.alpha = 0f;
            qteCanvasGroup.gameObject.SetActive(false); // prepare it to be shown later
        }
    }

    private void Update()
    {
        if (!qteActive || isLookingBack) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            float currentTime = Time.time;
            bool success = currentTime <= qteStartTime + validLookBackTime;


            StartCoroutine(PerformLookBack(success));
        }
        
        if (qteActive && !isLookingBack && Time.time > qteStartTime + validLookBackTime)
        {
            QTEFailed("Player did not respond in time.");
        }
    }

    public void StartQTE()
    {
        if (cameraTransform == null) cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("Cannot start QTE: No camera found.");
            return;
        }

        qteStartTime = Time.time;
        qteActive = true;

        ShowQTEVisual(true);
        Debug.Log("QTE Started. Press Q to look back at the right time!");
    }

    private void EndQTE()
    {
        qteActive = false;
        ShowQTEVisual(false);
    }

    private void ShowQTEVisual(bool show)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        
        if (qteCanvasGroup != null && show)
            qteCanvasGroup.gameObject.SetActive(true);
        fadeRoutine = StartCoroutine(FadeQTEVisual(show));
    }

    private IEnumerator FadeQTEVisual(bool show)
    {
        if (qteCanvasGroup == null) yield break;

        qteCanvasGroup.gameObject.SetActive(true);
        float targetAlpha = show ? 1f : 0f;
        float startAlpha = qteCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            qteCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        qteCanvasGroup.alpha = targetAlpha;

        if (!show) qteCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator PerformLookBack(bool wasSuccessful)
    {
        isLookingBack = true;

        originalRotation = cameraTransform.localRotation;
        lookBackRotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y + 180f, originalRotation.eulerAngles.z);

        yield return RotateCamera(originalRotation, lookBackRotation);
        yield return new WaitForSeconds(lookDuration);
        yield return RotateCamera(lookBackRotation, originalRotation);

        if (wasSuccessful)
            QTESuccess();
        else
            QTEFailed("Timing incorrect.");

        isLookingBack = false;
    }

    private IEnumerator RotateCamera(Quaternion from, Quaternion to)
    {
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cameraTransform.localRotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime * lookRotationSpeed;
            yield return null;
        }

        cameraTransform.localRotation = to;
    }

    private void QTESuccess()
    {
        EndQTE();
        Debug.Log("QTE Success: Player looked back correctly.");
        // Add feedback/trigger here
    }

    private void QTEFailed(string reason)
    {
        EndQTE();
        Debug.Log($"QTE Failed: {reason}");
        // Add failure consequence here
    }
}
