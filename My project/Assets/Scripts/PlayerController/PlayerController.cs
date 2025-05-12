using UnityEngine;

public class PlayerController : IUpdatable
{
    private readonly Transform playerTransform;
    private readonly InputManager inputHandler;
    private readonly CollisionManager collisionHandler;
    private Transform cameraTransform;

    private Vector3 velocity;
    private Vector3 cachedMove = new Vector3();
    private  Vector3 cachedVerticalMove = new Vector3();

    private readonly float moveSpeed;
    private readonly float jumpForce;
    private const float gravity = -20f;

    private readonly Transform RespawnPosition;

    public PlayerController(Transform playerTransform, InputManager inputHandler, CollisionManager collisionHandler, float moveSpeed, float jumpForce, Transform Respawn)
    {
        this.playerTransform = playerTransform;
        this.inputHandler = inputHandler;
        this.collisionHandler = collisionHandler;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
        this.RespawnPosition = Respawn; 
    }

    public void SetCameraTransform(Transform camTransform)
    {
        cameraTransform = camTransform;
    }

    public void Tick(float deltaTime)
    {
        HandleGravity(deltaTime);
        ApplyVerticalMovement(deltaTime);

        Vector3 moveInput = inputHandler.MoveInput;
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Move(deltaTime, moveInput);
            AlignWithCamera(deltaTime);
        }

        if (inputHandler.JumpPressed && collisionHandler.IsGrounded)
        {
            velocity.y = jumpForce;
        }

        // Si tocamos obstáculo o bala, aplicar empuje
        if (collisionHandler.IsTouchingObstacle)
        {
            ApplyExternalPush(collisionHandler.LastObstacleDirection, 300f, deltaTime);
        }

        if (collisionHandler.IsTouchingBullet)
        {
            ApplyExternalPush(collisionHandler.LastBulletDirection, 400f, deltaTime);
        }
    }
    private void ApplyExternalPush(Vector3 direction, float force, float deltaTime)
    {
        playerTransform.position += direction.normalized * force * deltaTime;
    }
    private void Move(float deltaTime, Vector3 inputVector)
    {
        cachedMove.Set(
            playerTransform.right.x * inputVector.x + playerTransform.forward.x * inputVector.z,
            0f,
            playerTransform.right.z * inputVector.x + playerTransform.forward.z * inputVector.z
        );

        cachedMove *= moveSpeed * deltaTime;
        playerTransform.position += cachedMove;
    }

    private void HandleGravity(float deltaTime)
    {
        if (collisionHandler.IsGrounded && velocity.y <= 0f)
        {
            // Si estamos tocando el suelo y cayendo, anclamos Y en 0
            velocity.y = 0f;
        }
        else
        {
            // Acumular gravedad
            velocity.y += gravity * deltaTime;
        }
    }

    private void ApplyVerticalMovement(float deltaTime)
    {
        cachedVerticalMove.Set(0f, velocity.y * deltaTime, 0f);
        playerTransform.position += cachedVerticalMove;

        if (playerTransform.position.y < -10f)
        {
            playerTransform.position = RespawnPosition.position;
            velocity.y = 0f;
        }
    }

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