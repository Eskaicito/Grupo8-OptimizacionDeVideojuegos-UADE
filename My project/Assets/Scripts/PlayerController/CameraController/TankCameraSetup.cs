using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Setup de la cámara estilo tanque.
/// Se encarga de crear y configurar la lógica de la cámara, par registrarla en el CustomUpdateManager
/// </summary>
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
