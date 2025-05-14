using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Setup de la c�mara estilo tanque.
/// Se encarga de crear y configurar la l�gica de la c�mara, par registrarla en el CustomUpdateManager
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
