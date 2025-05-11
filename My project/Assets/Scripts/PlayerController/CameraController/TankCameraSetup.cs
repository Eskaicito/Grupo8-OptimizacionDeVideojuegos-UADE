using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraSetup : MonoBehaviour
{
    [SerializeField] private Transform target;
    private TankCamera cameraLogic;

    void Awake()
    {
        cameraLogic = new TankCamera(transform, target);
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(cameraLogic);
    }

    //void OnDestroy()
    //{
    //    var updateManager = FindFirstObjectByType<CustomUpdateManager>();
    //    updateManager?.Unregister(cameraLogic);
    //}
}
