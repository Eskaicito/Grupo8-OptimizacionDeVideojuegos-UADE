using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject _loadingText;

    private void Awake()
    {
        if(_loadingText != null)
        {
            _loadingText.SetActive(false);
        }
        StartCoroutine(LoadSceneAsync());
    }

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
