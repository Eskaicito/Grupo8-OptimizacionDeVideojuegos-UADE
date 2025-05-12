using UnityEngine;

public class PlayerController : IUpdatable
{
    private readonly Transform playerTransform;
    private readonly InputManager inputHandler;
    private readonly CollisionManager collisionHandler;
    private Transform cameraTransform;

    private Vector3 velocity;
    private Vector3 cachedMove = new Vector3();
    private Vector3 cachedVerticalMove = new Vector3();
    private Vector3 cachedForward = new Vector3();
    private Vector3 cachedRight = new Vector3();



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
        cachedForward = playerTransform.forward;
        cachedRight = playerTransform.right;

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
        NormalizeSafe(ref direction);
        playerTransform.position += direction * force * deltaTime;
    }

    private void Move(float deltaTime, Vector3 inputVector)
    {
        cachedMove.Set(
            cachedRight.x * inputVector.x + cachedForward.x * inputVector.z,
            0f,
            cachedRight.z * inputVector.x + cachedForward.z * inputVector.z
        );

        if (cachedMove.sqrMagnitude > 0.01f)
        {
            if (!collisionHandler.CheckWall(cachedMove))
            {
                cachedMove *= moveSpeed * deltaTime;
                playerTransform.position += cachedMove;
            }
        }
    }

    private void HandleGravity(float deltaTime)
    {
        if (collisionHandler.IsGrounded)
        {
            if (velocity.y < 0f)
                velocity.y = 0f;
        }
        else
        {
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
    private static void NormalizeSafe(ref Vector3 vector)
    {
        float mag = vector.magnitude;
        if (mag > 0.0001f)
        {
            float invMag = 1f / mag;
            vector.x *= invMag;
            vector.y *= invMag;
            vector.z *= invMag;
        }
    }
}
