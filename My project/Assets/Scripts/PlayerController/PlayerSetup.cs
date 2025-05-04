using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private PlayerController playerLogic;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    
    void Awake()
    {
        
        playerLogic = new PlayerController(transform, groundMask, wallMask, moveSpeed, jumpForce);

        var cam = Camera.main;
        if (cam != null)
        {
            playerLogic.SetCameraTransform(cam.transform);
        }

        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(playerLogic);
    }

}
