using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class WallStick : MonoBehaviour
{
    [Header("Wall Sticking")]
    public LayerMask whatIsWall;
    public float wallCheckDistance = 1f;
    public float wallStickDuration = 5f; // Duration the player can stick to the wall
    public float wallStickCooldown = 2f; // Cooldown before player can wall-stick again

    [Header("Camera Tilt")]
    public Camera playerCamera;
    public float tiltAngle = 30f;
    public float tiltSpeed = 5f;
    private float currentTilt = 0f;

    [Header("References")]
    private PlayerMovement pm;
    private Rigidbody rb;
    private Vector3 stickForwardDirection;
    private bool isWallSticking;
    private bool canWallStick = true; // New flag to control wall-sticking ability
    private float wallStickCooldownTimer;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera reference is missing in WallStick.");
        }
    }

    private void Update()
    {
        CheckForWall();

        // Update wall stick cooldown if wall-sticking is unavailable
        if (!canWallStick)
        {
            wallStickCooldownTimer -= Time.deltaTime;
            if (wallStickCooldownTimer <= 0f)
            {
                canWallStick = true; // Reactivate wall-sticking after cooldown
            }
        }

        if (
            Input.GetMouseButton(1)
            && !pm.groundedPlayer
            && canWallStick
            && (wallLeft || wallRight)
        )
        {
            if (!isWallSticking)
            {
                StartWallStick();
            }
            else
            {
                StickToWall();
            }
        }
        else
        {
            StopWallStick();
        }
    }

    private void CheckForWall()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        wallLeft = Physics.Raycast(
            origin,
            -transform.right,
            out leftWallHit,
            wallCheckDistance,
            whatIsWall
        );
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

    private void StartWallStick()
    {
        isWallSticking = true;
        rb.useGravity = false;

        // Capture the forward direction only once at the start of the stick
        if (stickForwardDirection == Vector3.zero)
        {
            stickForwardDirection = transform.forward;
        }
    }

    private void StickToWall()
    {
        // Prevent player from moving horizontally
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        // Calculate and apply camera tilt
        float targetTilt = wallLeft ? -tiltAngle : tiltAngle;
        currentTilt = Mathf.LerpAngle(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);

        Quaternion originalRotation = playerCamera.transform.localRotation;
        playerCamera.transform.localRotation = Quaternion.Euler(
            originalRotation.eulerAngles.x,
            originalRotation.eulerAngles.y,
            currentTilt
        );

        // Allow player to move only forward along stickForwardDirection
        Vector3 stickMovement = stickForwardDirection * pm.playerSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + stickMovement);
    }

    private void StopWallStick()
    {
        isWallSticking = false;
        rb.useGravity = true;

        // Reset tilt to 0 gradually
        currentTilt = Mathf.LerpAngle(currentTilt, 0f, tiltSpeed * Time.deltaTime);

        Quaternion originalRotation = playerCamera.transform.localRotation;
        playerCamera.transform.localRotation = Quaternion.Euler(
            originalRotation.eulerAngles.x,
            originalRotation.eulerAngles.y,
            currentTilt
        );

        stickForwardDirection = Vector3.zero; // Reset forward direction when wall-sticking stops
    }
}
