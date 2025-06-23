using UnityEngine;

public class MotionObjectController : MonoBehaviour
{
    public float moveSpeed = 5f;    // Speed of movement
    public float jumpHeight = 2f;  // Height of the jump
    public float gravity = -9.81f; // Gravity force
    public Transform cameraTransform; // Reference to the main camera
    public Transform playerTransform; // Reference to the player

    private Vector3 moveDirection;  // For movement
    private Vector3 velocity;       // For gravity and jumping
    private bool isGrounded;        // Check if grounded

    private CharacterController controller; // For motion collision

    void Start()
    {
        // Add a CharacterController to the motion object
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("CharacterController component is missing!");
        }
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keep grounded
        }

        // Get camera's Y rotation
        float cameraYRotation = cameraTransform.eulerAngles.y;

        // Smoothly align the motion object's rotation with the camera
        Quaternion targetRotation = Quaternion.Euler(0, cameraYRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

        // Joystick input for movement
        float x = Input.GetAxis("Horizontal"); // Sideways movement
        float z = Input.GetAxis("Vertical");   // Forward/backward movement

        // Calculate movement direction relative to motion object's orientation
        moveDirection = transform.right * x + transform.forward * z;

        // Apply movement
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Jump physics
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Update the player's position and rotation to match the motion object
        if (playerTransform != null)
        {
            playerTransform.position = transform.position;

        }
    }
}
