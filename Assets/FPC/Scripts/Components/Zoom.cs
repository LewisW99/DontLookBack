using UnityEngine;


public class Zoom : MonoBehaviour
{
    Camera playerCamera;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float currentZoom;
    public float sensitivity = 10;

    private float targetFOV;
    void Awake()
    {
        // Get the camera on this gameObject and the defaultZoom.
        playerCamera = GetComponent<Camera>();
        if (playerCamera)
        {
            defaultFOV = playerCamera.fieldOfView;
        }
    }

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        targetFOV = defaultFOV;
        playerCamera.fieldOfView = defaultFOV;
    }
    void Update()
    {
        // Set the target FOV based on right mouse input
        if (Input.GetMouseButton(1)) // Right mouse button held
        {
            targetFOV = maxZoomFOV;
        }
        else
        {
            targetFOV = defaultFOV;
        }

        // Smooth transition to target FOV
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, sensitivity * Time.deltaTime);
    }
}
