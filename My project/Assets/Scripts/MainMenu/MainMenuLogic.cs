using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : IUpdatable
{
    private readonly Camera camera;
    private readonly Transform playButton;
    private readonly Transform exitButton;
    private readonly LayerMask layerMask;

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

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
            {
                if (hit.transform == playButton)
                {
                    PlayGame();
                }
                else if (hit.transform == exitButton)
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
