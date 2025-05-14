using UnityEngine;

/// <summary>
/// Controlador principal del jugador. Gestiona el movimiento, salto, gravedad, empujes externos, 
/// interacción con obstáculos y salida del juego.
/// Se ejecuta mediante un sistema de actualización manual a través de IUpdatable
/// </summary>

public class PlayerController : IUpdatable
{
    private readonly Transform playerTransform;
    private readonly InputManager inputHandler;
    private readonly CollisionManager collisionHandler;
    private Transform cameraTransform;

    // Caché de movimientos para minimizar asignaciones de memoria
    private Vector3 velocity;
    private Vector3 cachedMove = new Vector3();
    private Vector3 cachedVerticalMove = new Vector3();
    private Vector3 cachedForward = new Vector3();
    private Vector3 cachedRight = new Vector3();

    private readonly float moveSpeed;
    private readonly float jumpForce;
    private const float gravity = -20f;

    private readonly Transform RespawnPosition;

    /// <summary>
    /// El Constructor que recibe todas las dependencias necesarias desde el exterior.
    /// </summary>
    public PlayerController(Transform playerTransform, InputManager inputHandler, CollisionManager collisionHandler, float moveSpeed, float jumpForce, Transform Respawn)
    {
        this.playerTransform = playerTransform;
        this.inputHandler = inputHandler;
        this.collisionHandler = collisionHandler;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
        this.RespawnPosition = Respawn;
    }
    /// <summary>
    /// Esto para alinear l c[amara con la rotaci[on del jugador
    /// </summary>
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

       
        if (collisionHandler.IsInWinZone || inputHandler.ExitPressed)
        {
           
            Application.Quit();
        }
    }
    /// <summary>
    /// Aplica una fuerza de empuje al jugador en la dirección indicada. Es para cuando algo lo golpee
    /// </summary>
    private void ApplyExternalPush(Vector3 direction, float force, float deltaTime)
    {
        NormalizeSafe(ref direction);
        playerTransform.position += direction * force * deltaTime;
    }

    /// <summary>
    /// Calcula y aplica el movimiento horizontal del jugador en base al input. Tambien para el choque de paredes
    /// </summary>
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

    /// <summary>
    /// Calcular la gravedad del jugador. Si toca el suelo, es 0.
    /// </summary>
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
    /// <summary>
    /// Aplica el movimiento vertical acumulado. También resetea la posición si el jugador cae fuera del nivel.
    /// </summary>
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

    /// <summary>
    /// Ajusta la rotación del jugador para que siempre mire hacia la dirección de la cámara.
    /// </summary>
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
    /// <summary>
    /// Para normalizar vectores
    /// </summary>
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
