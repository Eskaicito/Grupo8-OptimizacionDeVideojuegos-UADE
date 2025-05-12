using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private PlayerController playerLogic;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
        
        //if (playerRigidbody != null)
        //{
        //    playerRigidbody.isKinematic = true;
        //    playerRigidbody.useGravity = false;
        //}

        playerLogic = new PlayerController(transform, playerRigidbody, groundMask, moveSpeed, jumpForce);

        var cam = Camera.main;
        if (cam != null)
        {
            playerLogic.SetCameraTransform(cam.transform);
        }

        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(playerLogic);
    }
}
