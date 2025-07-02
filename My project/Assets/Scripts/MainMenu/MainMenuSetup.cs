using UnityEngine;

public class MainMenuSetup : MonoBehaviour
{
    [SerializeField] private Camera cameras;
    [SerializeField] private Transform playButton;
    [SerializeField] private Transform exitButton;
    [SerializeField] private LayerMask buttonLayerMask;
    [SerializeField] private GameObject splashScreenUI; 

    private void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();

        var mainMenuLogic = new MainMenuLogic(cameras, playButton, exitButton, buttonLayerMask, splashScreenUI);
        updateManager.Register(mainMenuLogic);
    }
}
