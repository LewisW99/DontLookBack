using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;
    
    [Header("Yaw Clamp (Left/Right)")]
    public float minYaw = -70f;
    public float maxYaw = 70f;

    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse delta input
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;

        // Clamp vertical look (pitch)
        velocity.y = Mathf.Clamp(velocity.y, -90f, 90f);

        // Clamp horizontal look (yaw)
        velocity.x = Mathf.Clamp(velocity.x, minYaw, maxYaw);

        // Apply rotations
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right); // Camera (up/down)
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);     // Player body (left/right)
    }
}
