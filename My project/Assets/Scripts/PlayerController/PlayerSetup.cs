using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private PlayerController playerLogic;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;

    void Awake()
    {
        
        playerLogic = new PlayerController(transform, groundMask, wallMask);

        
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(playerLogic);
    }
}
