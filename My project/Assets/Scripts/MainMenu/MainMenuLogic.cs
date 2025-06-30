using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : IUpdatable
{
    private readonly Camera camera;
    private readonly Transform playButton;
    private readonly Transform exitButton;
    private readonly LayerMask layerMask;
    private readonly RaycastHit[] raycastHits = new RaycastHit[1];

    public MainMenuLogic(Camera camera, Transform playButton, Transform exitButton, LayerMask layerMask)
    {
        this.camera = camera;
        this.playButton = playButton;
        this.exitButton = exitButton;
        this.layerMask = layerMask;
    }

    public void Tick(float deltaTime)
    {
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
