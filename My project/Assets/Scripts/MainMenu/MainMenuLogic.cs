using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : IUpdatable
{
    private readonly Camera camera;
    private readonly Transform playButton;
    private readonly Transform exitButton;
    private readonly LayerMask layerMask;
    private readonly RaycastHit[] raycastHits = new RaycastHit[1];

    private readonly GameObject splashScreenUI;
    private float splashTimer = 0f;
    private bool splashHidden = false;

    public MainMenuLogic(Camera camera, Transform playButton, Transform exitButton, LayerMask layerMask, GameObject splashScreenUI)
    {
        this.camera = camera;
        this.playButton = playButton;
        this.exitButton = exitButton;
        this.layerMask = layerMask;
        this.splashScreenUI = splashScreenUI;
    }

    public void Tick(float deltaTime)
    {
        
        if (!splashHidden)
        {
            splashTimer += deltaTime;
            if (splashTimer >= 4f)
            {
                splashScreenUI.SetActive(false);
                splashHidden = true;
            }
            return; 
        }

   
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 100f, layerMask);

            if (hitCount > 0)
            {
                Transform hitTransform = raycastHits[0].transform;

                if (hitTransform == playButton)
                {
                    PlayGame();
                }
                else if (hitTransform == exitButton)
                {
                    ExitGame();
                }
            }
        }
    }

    private void PlayGame()
    {
        LoadingData.SceneToLoad = "Level1";
        SceneManager.LoadScene("LoadingScreen");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
