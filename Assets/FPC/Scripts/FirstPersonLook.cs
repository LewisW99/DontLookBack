using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform character;
    [SerializeField] Transform cameraHolder; // Empty GameObject to hold the camera (for bobbing)

    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    [Header("Head Bobbing")]
    public float bobFrequency = 1.5f;
    public float bobAmplitude = 0.05f;
    public FirstPersonMovement movementScript;
    private float bobTimer = 0f;
    private Vector3 initialCameraLocalPos;

    [SerializeField] PauseMenu pauseMenu; // Reference to the PauseMenu script

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;

    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (cameraHolder == null)
            cameraHolder = transform; // fallback to camera if not assigned

        initialCameraLocalPos = cameraHolder.localPosition;
    }

    void Update()
    {
        if (!pauseMenu.isPaused )
        {
            // Mouse look
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;

            transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

            // Head bobbing
            HandleHeadBobbing();
        }

    }

    void HandleHeadBobbing()
    {
        if (movementScript == null) return;

        Vector3 velocity = movementScript.GetComponent<Rigidbody>().linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

        if (horizontalVelocity.magnitude > 0.1f && movementScript.IsRunning || horizontalVelocity.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * (movementScript.IsRunning ? bobFrequency * 1.5f : bobFrequency);
            float bobOffset = Mathf.Sin(bobTimer * Mathf.PI * 2) * bobAmplitude;

            Vector3 newPos = initialCameraLocalPos + new Vector3(0, bobOffset, 0);
            cameraHolder.localPosition = newPos;
        }
        else
        {
            // Reset when idle
            bobTimer = 0;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, initialCameraLocalPos, Time.deltaTime * 5f);
        }
    }
}
