using UnityEngine;

/// <summary>
/// Setup del jugador. Encargado de instanciar y conectar todas las dependencias necesarias.
/// Registralos componentes al CustomUpdateManager.
/// </summary>
public class PlayerSetup : MonoBehaviour
{
    // Máscaras de colisión
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] private LayerMask winZoneMask;

    // Parámetros de movimiento 
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

        // Asigna la cámara principal si existe, para que el jugador alinee su rotación con la cámara
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
