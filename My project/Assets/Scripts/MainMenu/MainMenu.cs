using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playGame;
    [SerializeField] private Button _exitGame;

    private void Awake()
    {
        _playGame.onClick.AddListener(PlayGame);
        _exitGame.onClick.AddListener(ExitGame);
    }

    private void PlayGame()
    {
        LoadingData.SceneToLoad = "TestRoomZ1"; // Set the scene to load in LoadingData
        // Load the game scene
        SceneManager.LoadScene("LoadingScreen");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
