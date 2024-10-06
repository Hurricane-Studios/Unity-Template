using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Sensitivity controls for mouse movement
    public float mouseSensitivity = 100f;

    // Reference to the main camera
    private Transform mainCamera;

    // Variable to store the current rotation around the X-axis
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to the main camera in the scene
        mainCamera = Camera.main.transform;

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input from the Input Manager
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera up and down (Y-axis)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit the up and down rotation

        // Apply rotation to the main camera
        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the main camera left and right based on mouseX input
        transform.Rotate(Vector3.up * mouseX);
    }
}
