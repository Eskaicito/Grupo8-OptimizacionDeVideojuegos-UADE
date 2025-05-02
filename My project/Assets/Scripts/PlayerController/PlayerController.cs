using UnityEngine;

public class PlayerController : IUpdatable
{
    private Transform playerTransform;
    private Vector3 velocity;

    private float moveSpeed = 5f;
    private float jumpForce = 7f;
    private float gravity = -20f;

    private float groundCheckDistance = 0.3f;
    private float wallCheckDistance = 0.1f;
    private float characterRadius = 0.3f;

    private bool isGrounded;

    private RaycastHit[] hitResults = new RaycastHit[4];
    private LayerMask groundMask;
    private LayerMask wallMask;

    public PlayerController(Transform transform, LayerMask groundMask, LayerMask wallMask)
    {
        playerTransform = transform;
        this.groundMask = groundMask;
        this.wallMask = wallMask;
    }

    public void Tick(float deltaTime)
    {
        isGrounded = CheckGrounded();
        HandleMovement(deltaTime);
        HandleGravity(deltaTime);
        ApplyMovement(deltaTime);
    }

    private void HandleMovement(float deltaTime)
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 moveDir = playerTransform.right * input.x + playerTransform.forward * input.z;

        
        if (moveDir != Vector3.zero && !CheckWall(moveDir))
        {
            playerTransform.position += moveDir * moveSpeed * deltaTime;
        }

        
        isGrounded = CheckGrounded();

       
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }
    }

    private void HandleGravity(float deltaTime)
    {
        if (!isGrounded)
        {
            velocity.y += gravity * deltaTime;
        }
    }

    private void ApplyMovement(float deltaTime)
    {
        Vector3 verticalMove = new Vector3(0, velocity.y, 0) * deltaTime;
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
        Vector3 direction = Vector3.down;

        int hits = Physics.SphereCastNonAlloc(origin, characterRadius, direction, hitResults, groundCheckDistance, groundMask);
        return hits > 0;
    }

    private bool CheckWall(Vector3 direction)
    {
        Vector3 origin = playerTransform.position + Vector3.up * 0.05f;
        direction = direction.normalized;

        int hits = Physics.SphereCastNonAlloc(origin, characterRadius, direction, hitResults, wallCheckDistance, wallMask);
        return hits > 0;
    }
}
