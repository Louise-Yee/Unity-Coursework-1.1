using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class WallStick : MonoBehaviour
{
    [Header("Wall Sticking")]
    public LayerMask whatIsWall;
    public float wallCheckDistance = 1f;

    [Header("References")]
    private PlayerMovement pm;
    private Rigidbody rb;
    private Vector3 stickForwardDirection;

    // Camera reference
    public Camera playerCamera;
    public float tiltAngle = 30f; // Angle to tilt the camera
    public float tiltSpeed = 5f; // Speed of camera tilt transition
    private float currentTilt = 0f; // Track the current tilt
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Wall Bounce")]
    public float bounceForce = 8f; // Force applied when bouncing off a wall
    public float horizontalBounceForce = 5f; // Horizontal force applied when bouncing off a wa

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        // Ensure the playerCamera is assigned in the inspector
        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera reference is missing in WallStick.");
        }
    }

    private void Update()
    {
        CheckForWall();
        if (Input.GetMouseButton(1))
        {
            StickToWall();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BounceOffWall();
        }
    }

    private void CheckForWall()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f; // Adjust height based on your character's collider

        // Check for left wall
        wallLeft = Physics.Raycast(
            origin,
            -transform.right,
            out leftWallHit,
            wallCheckDistance,
            whatIsWall
        );

        // Check for right wall
        wallRight = Physics.Raycast(
            origin,
            transform.right,
            out rightWallHit,
            wallCheckDistance,
            whatIsWall
        );

        // Debug raycasts
        Debug.DrawRay(
            origin,
            -transform.right * wallCheckDistance,
            wallLeft ? Color.green : Color.red
        );
        Debug.DrawRay(
            origin,
            transform.right * wallCheckDistance,
            wallRight ? Color.green : Color.red
        );
    }

    private void StickToWall()
    {
        if (!pm.groundedPlayer && (wallLeft || wallRight) && Input.GetMouseButton(1))
        {
            // Set player speed and disable gravity for wall-sticking
            // pm.playerSpeed = pm.isRunning ? 15f : 8f;
            rb.useGravity = false;

            // Freeze horizontal movement
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            // Capture the forward direction on initial stick
            if (stickForwardDirection == Vector3.zero)
            {
                stickForwardDirection = transform.forward;
            }

            // Apply tilt based on wall side
            float targetTilt = wallLeft ? -tiltAngle : tiltAngle;
            currentTilt = Mathf.LerpAngle(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);

            Quaternion originalRotation = playerCamera.transform.localRotation;
            playerCamera.transform.localRotation = Quaternion.Euler(
                originalRotation.eulerAngles.x,
                originalRotation.eulerAngles.y,
                currentTilt
            );

            // Ensure player only moves forward in the stickForwardDirection
            Vector3 stickMovement = stickForwardDirection * pm.playerSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + stickMovement);
        }
        else
        {
            rb.useGravity = true;
            currentTilt = Mathf.LerpAngle(currentTilt, 0f, tiltSpeed * Time.deltaTime);

            Quaternion originalRotation = playerCamera.transform.localRotation;
            playerCamera.transform.localRotation = Quaternion.Euler(
                originalRotation.eulerAngles.x,
                originalRotation.eulerAngles.y,
                currentTilt
            );

            // Reset the stick forward direction when the player stops wall-sticking
            stickForwardDirection = Vector3.zero;
        }
    }

    private void BounceOffWall()
    {
        if (!pm.groundedPlayer && (wallLeft || wallRight) && !Input.GetMouseButton(1))
        {
            // Apply an upward or downward bounce force when the player taps space
            float bounceDirection = 1f;

            // Clear current movement and apply bounce force
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Clear current y velocity

            // Calculate bounce direction based on which wall is being touched
            Vector3 bounceDirectionVector = Vector3.up * bounceForce * bounceDirection; // Vertical force
            if (wallLeft)
            {
                // If touching left wall, bounce to the right
                bounceDirectionVector += transform.right * horizontalBounceForce;
            }
            else if (wallRight)
            {
                // If touching right wall, bounce to the left
                bounceDirectionVector += -transform.right * horizontalBounceForce;
            }

            // Apply the combined vertical and horizontal bounce force
            rb.AddForce(bounceDirectionVector, ForceMode.Impulse);

            // Reset wall stick state
            rb.useGravity = true;
        }
    }
}
