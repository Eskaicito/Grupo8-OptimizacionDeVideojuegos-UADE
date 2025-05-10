using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSetup : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnerPoints;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private float fireRate = 1f;

    private static readonly int BulletChildIndex = 0;

    void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();

        foreach (Transform spawner in spawnerPoints)
        {
            var bullet = spawner.GetChild(BulletChildIndex).transform;

            var logic = new BulletLogic(
                bullet,
                spawner.forward,
                bulletSpeed,
                bulletLifetime,
                spawner,
                fireRate
            );

            updateManager.Register(logic);
        }
    }
}
