using UnityEngine;

/// <summary>
/// Setup del jugador. Encargado de instanciar y conectar todas las dependencias necesarias.
/// Registralos componentes al CustomUpdateManager.
/// </summary>
public class PlayerSetup : MonoBehaviour
{
    // M�scaras de colisi�n
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] private LayerMask winZoneMask;

    // Par�metros de movimiento 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    // Transform de respawn
    [SerializeField] private Transform respawn;

    private void Awake()
    {
        var inputHandler = new InputManager();
        var collisionHandler = new CollisionManager(
            transform,
            groundMask,
            wallMask,
            obstacleMask,
            bulletMask,
            winZoneMask 
        );

        var playerLogic = new PlayerController(
            transform,
            inputHandler,
            collisionHandler,
            moveSpeed,
            jumpForce,
            respawn
        );

        // Asigna la c�mara principal si existe, para que el jugador alinee su rotaci�n con la c�mara
        var cam = Camera.main;
        if (cam != null)
        {
            playerLogic.SetCameraTransform(cam.transform);
        }

        // Registra todas las instancias en el CustomUpdateManager para ser actualizadas manualmente
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(inputHandler);
        updateManager.Register(collisionHandler);
        updateManager.Register(playerLogic);
    }
}
