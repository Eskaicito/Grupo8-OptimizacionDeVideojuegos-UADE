using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraSetup : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask groundMask;
    private TankCamera cameraLogic;

    void Awake()
    {
        cameraLogic = new TankCamera(transform, target, groundMask);
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(cameraLogic);
    }
}
