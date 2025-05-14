using UnityEngine;


//<sumary>
// Esta clase se encarga de detectar colisiones entre el jugador y varios objetos en el juego.

public class CollisionManager : IUpdatable
{
    private readonly Transform playerTransform;
    private readonly LayerMask groundMask;
    private readonly LayerMask wallMask;
    private readonly LayerMask obstacleMask;
    private readonly LayerMask bulletMask;
    private readonly LayerMask winZoneMask;

    // Se utiliza un array de RaycastHit para almacenar los resultados de las colisiones.
    private readonly RaycastHit[] hitResults = new RaycastHit[1];

    public bool IsGrounded { get; private set; }
    public bool IsTouchingWall { get; private set; }
    public bool IsTouchingObstacle { get; private set; }
    public bool IsTouchingBullet { get; private set; }
    public bool IsInWinZone { get; private set; }

    public Vector3 LastObstacleDirection { get; private set; }
    public Vector3 LastBulletDirection { get; private set; }

    private static readonly Vector3 UpOffset = new Vector3(0f, 0.05f, 0f);

    // Constructor que inicializa las propiedades necesarias para la detección de colisiones.
    public CollisionManager(Transform playerTransform, LayerMask groundMask, LayerMask wallMask, LayerMask obstacleMask, LayerMask bulletMask, LayerMask winZoneMask)
    {
        this.playerTransform = playerTransform;
        this.groundMask = groundMask;
        this.wallMask = wallMask;
        this.obstacleMask = obstacleMask;
        this.bulletMask = bulletMask;
        this.winZoneMask = winZoneMask;
    }

    // Este metodo se llama manualmente desde el IUpdatable
    // Método que se llama cada frame para actualizar el estado de las colisiones.
    // Este método verifica si el jugador está en el suelo, tocando una pared, un obstáculo, una bala o en la zona de victoria.
    public void Tick(float deltaTime)
    {
        IsGrounded = CheckGrounded();
        IsTouchingWall = CheckWall(playerTransform.forward);
        IsTouchingObstacle = CheckObstacle(out Vector3 obstacleDir);
        LastObstacleDirection = obstacleDir;
        IsTouchingBullet = CheckBullet(out Vector3 bulletDir);
        LastBulletDirection = bulletDir;
        IsInWinZone = CheckWinZone(); 
    }

    // Comprobar si el jugador está en el suelo (groundMask).
    private bool CheckGrounded()
    {
        Vector3 origin = playerTransform.position + UpOffset;
        return Physics.SphereCastNonAlloc(origin, 0.3f, Vector3.down, hitResults, 0.3f, groundMask) > 0;
    }

    // Comprobar si el jugador está tocando una pared (wallMask).
    public bool CheckWall(Vector3 direction)
    {
        Vector3 origin = playerTransform.position + UpOffset;
        return Physics.SphereCastNonAlloc(origin, 0.3f, direction.normalized, hitResults, 0.2f, wallMask) > 0;
    }

    // Comprobar si el jugador está tocando un obstáculo (obstacleMask).
    private bool CheckObstacle(out Vector3 direction)
    {
        Vector3 origin = playerTransform.position + UpOffset;
        if (Physics.SphereCastNonAlloc(origin, 0.3f, playerTransform.forward, hitResults, 0.5f, obstacleMask) > 0)
        {
            direction = -playerTransform.forward;
            return true;
        }

        direction = Vector3.zero;
        return false;
    }

    // Comprobar si el jugador está tocando una bala (bulletMask).
    // Si hay, devuelve la dirección opuesta.
    private bool CheckBullet(out Vector3 direction)
    {
        Vector3 origin = playerTransform.position + UpOffset;
        if (Physics.SphereCastNonAlloc(origin, 0.3f, playerTransform.forward, hitResults, 0.5f, bulletMask) > 0)
        {
            direction = -playerTransform.forward;
            return true;
        }

        direction = Vector3.zero;
        return false;
    }

    // Comprobar si el jugador está en la zona de victoria (winZoneMask).
    // Si lo esta, TRUE y gana el juego
    private bool CheckWinZone()
    {
        Vector3 origin = playerTransform.position + UpOffset;
        return Physics.SphereCastNonAlloc(origin, 0.3f, Vector3.forward, hitResults, 0.5f, winZoneMask) > 0;
    }
}
