using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSetUp : MonoBehaviour
{
    [SerializeField] private CustomUpdateManager _customUpdateManager;
    [SerializeField] private bool _isGhostPlatform = false;
    [SerializeField] private bool _isTrapPlatform = false;
    [SerializeField] private Transform _player;

    private void Awake()
    {
        if(_isGhostPlatform)
        {
            GhostPlatforms ghostPlatform = GetComponent<GhostPlatforms>();
            if (ghostPlatform != null)
            {
                _customUpdateManager.Register(ghostPlatform);
            }
        }
        if (_isTrapPlatform)
        {
            TrapPlatforms trapPlatform = GetComponent<TrapPlatforms>();
            if (trapPlatform != null)
            {
                trapPlatform.SetPlayer(_player);
                _customUpdateManager.Register(trapPlatform);
            }
        }
    }
}
