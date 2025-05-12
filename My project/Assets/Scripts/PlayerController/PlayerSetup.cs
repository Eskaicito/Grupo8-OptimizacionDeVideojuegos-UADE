using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform respawn;  

    private void Awake()
    {
        var inputHandler = new InputManager();
        var collisionHandler = new CollisionManager(transform, groundMask, wallMask, obstacleMask, bulletMask);
        var playerLogic = new PlayerController(transform, inputHandler, collisionHandler, moveSpeed, jumpForce, respawn);

        var cam = Camera.main;
        if (cam != null)
        {
            playerLogic.SetCameraTransform(cam.transform);
        }

        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(inputHandler);
        updateManager.Register(collisionHandler);
        updateManager.Register(playerLogic);
    }
}