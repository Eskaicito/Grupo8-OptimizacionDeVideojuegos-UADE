using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// Este script es para cargar una escena de manera asíncrona y mostrar un texto de carga mientras se realiza la carga.
// Se utiliza la clase AsyncOperation para cargar la escena en segundo plano y permitir que el juego siga funcionando mientras se carga la nueva escena.

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject _loadingText;

    // Metodo awake se ejecuta al iniciar el script
    // Se utiliza para inicializar el objeto y cargar la escena de manera asíncrona.
    private void Awake()
    {
        if(_loadingText != null)
        {
            _loadingText.SetActive(false);
        }
        StartCoroutine(LoadSceneAsync());
    }


    // Este metodo se encarga de cargar la escena de manera asíncrona
    // Se utiliza la clase AsyncOperation para cargar la escena en segundo plano

    private IEnumerator LoadSceneAsync()
    {
        string sceneToLoad = LoadingData.SceneToLoad;


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        yield return null;

        float timer = 0f;

        while (asyncLoad.progress < 0.9f)
        {
            timer += Time.deltaTime;
   
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        asyncLoad.allowSceneActivation = true;
    }


}
