using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSetUp : MonoBehaviour
{
    [SerializeField] private CustomUpdateManager _customUpdateManager;
    [SerializeField] private Transform _player;

    private void Awake()
    {
       
        GhostPlatforms[] ghostPlatformsArray = FindObjectsByType<GhostPlatforms>(FindObjectsSortMode.None);
        foreach (var ghost in ghostPlatformsArray)
        {
            _customUpdateManager.Register(ghost);
        }

       
        TrapPlatforms[] trapPlatformsArray = FindObjectsByType<TrapPlatforms>(FindObjectsSortMode.None);
        foreach (var trap in trapPlatformsArray)
        {
            trap.SetPlayer(_player);
            _customUpdateManager.Register(trap);
        }
    }
}
