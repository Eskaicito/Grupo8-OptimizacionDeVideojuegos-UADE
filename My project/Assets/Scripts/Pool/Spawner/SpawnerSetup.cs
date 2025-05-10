using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSetup : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnerPoints;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private float fireRate = 1f;

    void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();

        foreach (var spawner in spawnerPoints)
        {
            var bulletGO = spawner.GetChild(0).gameObject;
            var bulletLogic = new BulletLogic(
                bulletGO.transform,
                spawner.forward,  
                bulletSpeed,
                bulletLifetime,
                spawner,
                fireRate
            );
            updateManager.Register(bulletLogic);
        }
    }
}
