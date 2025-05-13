using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private bool _persistBetweenScenes = true;
 

    private static MusicPlayer _instance;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (_persistBetweenScenes)
        {
            if(_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _audioSource.Play();

    }
}
