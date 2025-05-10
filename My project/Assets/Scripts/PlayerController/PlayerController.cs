using UnityEngine;

public class PlayerController : IUpdatable
{
    private Transform playerTransform;
    private Transform cameraTransform;

    private Vector3 velocity;
    private Vector3 inputVector;
    private Vector3 moveDirection;
    private Vector3 verticalMove;

    private float moveSpeed;
    private float jumpForce;
    private const float gravity = -20f;

    private const float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    private const float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private const float groundCheckDistance = 0.3f;
    private const float wallCheckDistance = 0.2f;
    private const float characterRadius = 0.3f;

    private bool isGrounded;

    private RaycastHit[] hitResults = new RaycastHit[4];
    private LayerMask groundMask;
    private LayerMask wallMask;

    private static readonly Vector3 UpOffset = new Vector3(0f, 0.05f, 0f);
    private static readonly Vector3 RespawnPosition = new Vector3(0f, 2f, 0f);
    private static readonly KeyCode JumpKey = KeyCode.Space;

    public PlayerController(Transform transform, LayerMask groundMask, LayerMask wallMask, float moveSpeed, float jumpForce)
    {
        playerTransform = transform;
        this.groundMask = groundMask;
        this.wallMask = wallMask;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
    }

    public void SetCameraTransform(Transform camTransform)
    {
        cameraTransform = camTransform;
    }

    public void Tick(float deltaTime)
    {
        isGrounded = CheckGrounded();

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= deltaTime;

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
    }

    private void ReadInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        inputVector.x = x;
        inputVector.y = 0f;
        inputVector.z = z;

        if (inputVector.sqrMagnitude > 1f)
            inputVector.Normalize();

        if (Input.GetKeyDown(JumpKey))
            jumpBufferCounter = jumpBufferTime;
    }

    private void HandleMovement(float deltaTime)
    {
        moveDirection = playerTransform.right * inputVector.x + playerTransform.forward * inputVector.z;

        if (moveDirection.sqrMagnitude > 0.01f && !CheckWall(moveDirection))
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
        float moveY = velocity.y * deltaTime;
        playerTransform.position += new Vector3(0f, moveY, 0f);

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

    private bool CheckWall(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return false;

        Vector3 origin = playerTransform.position + UpOffset;
        direction.Normalize();
        return Physics.SphereCastNonAlloc(origin, characterRadius, direction, hitResults, wallCheckDistance, wallMask) > 0;
    }

    private void AlignWithCamera(float deltaTime)
    {
        if (cameraTransform == null) return;

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, deltaTime * 10f);
    }
}