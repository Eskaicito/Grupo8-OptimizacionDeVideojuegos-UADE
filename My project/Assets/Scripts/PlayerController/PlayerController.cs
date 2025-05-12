using UnityEngine;

public class PlayerController : IUpdatable
{
    private readonly Transform playerTransform;
    private readonly Rigidbody playerRigidbody;
    private Transform cameraTransform; 

    private Vector3 velocity;
    private Vector3 inputVector;
    private Vector3 moveDirection;

    private readonly float moveSpeed;
    private readonly float jumpForce;
    private const float gravity = -20f;

    private float jumpBufferCounter;
    private float coyoteTimeCounter;

    private bool isGrounded;

    private readonly RaycastHit[] hitResults = new RaycastHit[4];
    private readonly LayerMask groundMask;
 

    private static readonly Vector3 UpOffset = new Vector3(0f, 0.05f, 0f);
    private static readonly Vector3 RespawnPosition = new Vector3(0f, 2f, 0f);

    private const float groundCheckDistance = 0.3f;
    //private const float wallCheckDistance = 0.2f;
    private const float characterRadius = 0.3f;

    private const float jumpBufferTime = 0.15f;
    private const float coyoteTime = 0.2f;

    public PlayerController(Transform playerTransform, Rigidbody playerRigidbody, LayerMask groundMask, float moveSpeed, float jumpForce)
    {
        this.playerTransform = playerTransform;
        this.playerRigidbody = playerRigidbody;
        this.groundMask = groundMask;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;

        playerRigidbody.isKinematic = true;
        playerRigidbody.useGravity = false;
    }

    public void SetCameraTransform(Transform camTransform)
    {
        cameraTransform = camTransform;
    }

    public void Tick(float deltaTime)
    {
        isGrounded = CheckGrounded();

        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - deltaTime;
        jumpBufferCounter -= deltaTime;

        ReadInput();

        if (inputVector.sqrMagnitude > 0.01f)
        {
            HandleMovement(deltaTime);
            AlignWithCamera(deltaTime);
        }

        HandleGravity(deltaTime);
        ApplyJump();
        ApplyVerticalMovement(deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Saliendo del juego...");
            Application.Quit();
        }
    }

    private void ReadInput()
    {
        inputVector.Set(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (inputVector.sqrMagnitude > 1f)
            inputVector.Normalize();

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
    }

    private void HandleMovement(float deltaTime)
    {
        moveDirection = playerTransform.right * inputVector.x + playerTransform.forward * inputVector.z;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            playerTransform.position += moveDirection * moveSpeed * deltaTime;
        }
    }

    private void HandleGravity(float deltaTime)
    {
        if (!isGrounded)
            velocity.y += gravity * deltaTime;
        else if (velocity.y < 0f)
            velocity.y = 0f;
    }

    private void ApplyJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            velocity.y = jumpForce;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    private void ApplyVerticalMovement(float deltaTime)
    {
        playerTransform.position += new Vector3(0f, velocity.y * deltaTime, 0f);

        if (playerTransform.position.y < -10f)
        {
            playerTransform.position = RespawnPosition;
            velocity.y = 0f;
        }
    }

    private bool CheckGrounded()
    {
        Vector3 origin = playerTransform.position + UpOffset;
        return Physics.SphereCastNonAlloc(origin, characterRadius, Vector3.down, hitResults, groundCheckDistance, groundMask) > 0;
    }

    //private bool CheckWall(Vector3 direction)
    //{
    //    Vector3 origin = playerTransform.position + UpOffset;
    //    return Physics.SphereCastNonAlloc(origin, characterRadius, direction.normalized, hitResults, wallCheckDistance, wallMask) > 0;
    //}

    private void AlignWithCamera(float deltaTime)
    {
        if (cameraTransform == null) return;

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, deltaTime * 10f);
        }
    }
}