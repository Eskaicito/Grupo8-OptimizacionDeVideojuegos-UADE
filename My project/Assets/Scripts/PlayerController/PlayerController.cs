using UnityEngine;

public class PlayerController : IUpdatable
{
    private Transform playerTransform;
    private Transform cameraTransform;

    private Vector3 velocity;
    private Vector3 inputVector = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 verticalMove = Vector3.zero;

    private float moveSpeed;
    private float jumpForce;
    private float gravity = -20f;

    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter = 0f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0f;

    private float groundCheckDistance = 0.3f;
    private float wallCheckDistance = 0.2f;
    private float characterRadius = 0.3f;

    private bool isGrounded;

    private RaycastHit[] hitResults = new RaycastHit[4];
    private LayerMask groundMask;
    private LayerMask wallMask;

    // Cache strings y teclas
    private static readonly string HorizontalAxis = "Horizontal";
    private static readonly string VerticalAxis = "Vertical";
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
        else if (coyoteTimeCounter > 0f)
            coyoteTimeCounter -= deltaTime;

        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= deltaTime;

        HandleMovement(deltaTime);
        HandleGravity(deltaTime);
        ApplyMovement(deltaTime);
        AlignWithCamera(deltaTime);
    }

    private void HandleMovement(float deltaTime)
    {
        inputVector.x = Input.GetAxisRaw(HorizontalAxis);
        inputVector.z = Input.GetAxisRaw(VerticalAxis);
        inputVector.y = 0f;
        inputVector = Vector3.ClampMagnitude(inputVector, 1f);

        moveDirection = playerTransform.right * inputVector.x + playerTransform.forward * inputVector.z;

        if (moveDirection != Vector3.zero && !CheckWall(moveDirection))
        {
            playerTransform.position += moveDirection * moveSpeed * deltaTime;
        }

        if (Input.GetKeyDown(JumpKey))
            jumpBufferCounter = jumpBufferTime;

        if (jumpBufferCounter > 0f && (isGrounded || coyoteTimeCounter > 0f))
        {
            velocity.y = jumpForce;
            isGrounded = false;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    private void HandleGravity(float deltaTime)
    {
        if (!isGrounded)
            velocity.y += gravity * deltaTime;
    }

    private void ApplyMovement(float deltaTime)
    {
        verticalMove.x = 0f;
        verticalMove.y = velocity.y * deltaTime;
        verticalMove.z = 0f;

        playerTransform.position += verticalMove;

        if (playerTransform.position.y < -10f)
        {
            playerTransform.position = new Vector3(0f, 2f, 0f);
            velocity.y = 0f;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
    }

    private bool CheckGrounded()
    {
        Vector3 origin = playerTransform.position + Vector3.up * 0.05f;
        return Physics.SphereCastNonAlloc(origin, characterRadius, Vector3.down, hitResults, groundCheckDistance, groundMask) > 0;
    }

    private bool CheckWall(Vector3 direction)
    {
        Vector3 origin = playerTransform.position + Vector3.up * 0.05f;
        direction.Normalize();
        return Physics.SphereCastNonAlloc(origin, characterRadius, direction, hitResults, wallCheckDistance, wallMask) > 0;
    }

    private void AlignWithCamera(float deltaTime)
    {
        if (cameraTransform == null || (Mathf.Approximately(inputVector.x, 0f) && Mathf.Approximately(inputVector.z, 0f)))
            return;

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, deltaTime * 10f);
        }
    }
}