using UnityEngine;

public class PlayerController : IUpdatable
{
    private readonly Transform playerTransform;
    private readonly Transform cameraTransform;
    private readonly InputManager inputHandler;
    private readonly CollisionManager collisionHandler;
    private readonly Transform respawnPosition;

    private readonly float moveSpeed;
    private readonly float jumpForce;
    private readonly float gravity;
    private readonly float acceleration;
    private readonly float deceleration;
    private readonly float turnSpeed;

    private Vector3 velocity;
    private Vector3 momentum;
    private float currentSpeed = 0f;

    private Vector3 externalPushVelocity = Vector3.zero;
    private const float externalPushDamping = 5f;

    public PlayerController(
        Transform playerTransform,
        InputManager inputHandler,
        CollisionManager collisionHandler,
        Transform respawnPosition,
        float moveSpeed,
        float jumpForce,
        float gravity,
        float acceleration,
        float deceleration,
        float turnSpeed,
        Transform cameraTransform)
    {
        this.playerTransform = playerTransform;
        this.inputHandler = inputHandler;
        this.collisionHandler = collisionHandler;
        this.respawnPosition = respawnPosition;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
        this.gravity = gravity;
        this.acceleration = acceleration;
        this.deceleration = deceleration;
        this.turnSpeed = turnSpeed;
        this.cameraTransform = cameraTransform;
    }

    public void Tick(float deltaTime)
    {
        HandleGravity(deltaTime);
        HandleMovement(deltaTime);
        HandleExternalPush(deltaTime);
        HandleJump();
        HandleImpacts();
        HandleGameState();
    }

    private void HandleGravity(float deltaTime)
    {
        velocity.y = PhysicsHelper.ApplyGravity(velocity.y, gravity, deltaTime, collisionHandler.IsGrounded);
        playerTransform.position = PhysicsHelper.ApplyVerticalMovement(playerTransform.position, velocity.y, deltaTime);

        float penetration = collisionHandler.GetGroundPenetration();
        if (penetration > 0f)
        {
            playerTransform.position += Vector3.up * penetration;
            velocity.y = 0f;
        }

        if (playerTransform.position.y < -10f)
        {
            playerTransform.position = respawnPosition.position;
            velocity.y = 0f;
        }
    }

    private void HandleMovement(float deltaTime)
    {
        Vector3 cameraForwardXZ = cameraTransform.forward;
        cameraForwardXZ.y = 0f;
        MathHelper.NormalizeSafe(ref cameraForwardXZ);

        Vector3 cameraRight = MathHelper.RightFromForward(cameraForwardXZ);

        Vector3 moveInput = inputHandler.MoveInput;
        bool hasInput = moveInput.sqrMagnitude > 0.01f;

        if (hasInput)
        {
            Vector3 desiredDirection = cameraRight * moveInput.x + cameraForwardXZ * moveInput.z;
            MathHelper.NormalizeSafe(ref desiredDirection);

            momentum = Vector3.Slerp(momentum, desiredDirection, turnSpeed * deltaTime);
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(momentum);
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, turnSpeed * deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * deltaTime);
        }

        if (currentSpeed > 0.01f && !collisionHandler.CheckWall(momentum))
        {
            playerTransform.position += momentum * currentSpeed * deltaTime;
        }
    }

    private void HandleExternalPush(float deltaTime)
    {
        if (externalPushVelocity.sqrMagnitude > 0.001f)
        {
            playerTransform.position += externalPushVelocity * deltaTime;
            externalPushVelocity = Vector3.Lerp(externalPushVelocity, Vector3.zero, externalPushDamping * deltaTime);
        }
    }

    private void HandleJump()
    {
        if (inputHandler.JumpPressed && collisionHandler.IsGrounded)
        {
            velocity.y = jumpForce;
        }
    }

    private void HandleImpacts()
    {
        if (collisionHandler.IsTouchingObstacle)
        {
            externalPushVelocity += PhysicsHelper.CalculateImpulsePush(collisionHandler.LastObstacleDirection, 5f, momentum);
        }

        if (collisionHandler.IsTouchingBullet)
        {
            externalPushVelocity += PhysicsHelper.CalculateImpulsePush(collisionHandler.LastBulletDirection, 5f, momentum);
        }
    }

    private void HandleGameState()
    {
        if (collisionHandler.IsInWinZone || inputHandler.ExitPressed)
        {
            Application.Quit();
        }
    }
}
