using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [Header("Collision Layers")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] private LayerMask winZoneMask;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 12f;
    [SerializeField] private float turnSpeed = 8f;

    [Header("Respawn Point")]
    [SerializeField] private Transform respawnPoint;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        if (updateManager == null)
        {
            Debug.LogError("CustomUpdateManager not found in the scene!");
            return;
        }

        // ✅ Input y Collision Managers
        var inputHandler = new InputManager();
        var collisionHandler = new CollisionManager(
            transform,
            groundMask,
            wallMask,
            obstacleMask,
            bulletMask,
            winZoneMask
        );

        var playerController = new PlayerController(
    transform,
    inputHandler,
    collisionHandler,
    respawnPoint,
    moveSpeed,
    jumpForce,
    gravity,
    acceleration,
    deceleration,
    turnSpeed,
    cameraTransform // ✅
);


        // ✅ Camera Controller (TankCamera)
        var tankCamera = new TankCamera(
     cameraTransform,
     transform,
     collisionHandler,
     inputHandler
 );

        // ✅ Registrar todo en UpdateManager
        updateManager.Register(inputHandler);
        updateManager.Register(collisionHandler);
        updateManager.Register(playerController);
        updateManager.Register(tankCamera);
    }
}
