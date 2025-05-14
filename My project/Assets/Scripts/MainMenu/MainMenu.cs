using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Clase que maneja el men� principal del juego.
// Contiene botones para iniciar el juego y salir de �l.
// Utiliza el sistema de eventos de Unity para manejar los clics en los botones.

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playGame;
    [SerializeField] private Button _exitGame;

    // M�todo que se llama al iniciar el script.
    // Aqu� se registran los eventos de clic para los botones.
    // Se utiliza el m�todo AddListener para asignar funciones a los eventos de clic.
    private void Awake()
    {
        _playGame.onClick.AddListener(PlayGame);
        _exitGame.onClick.AddListener(ExitGame);
    }

    // Metodo para cargar la escena del juego
    private void PlayGame()
    {
        LoadingData.SceneToLoad = "Level1";
        SceneManager.LoadScene("LoadingScreen");
    }

    // Metodo para salir del juego
    private void ExitGame()
    {
        Application.Quit();
    }
}
