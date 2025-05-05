using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyShooterSetup : MonoBehaviour
{
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private ProjectilePoolManager poolManager;
    private EnemyShooter shooter;

    void Awake()
    {
       
        shooter = new EnemyShooter(transform, fireRate, OnFire);

        
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(shooter);
    }

  
   

    private void OnFire(Vector3 position, Vector3 direction)
    {
        poolManager.SpawnProjectile(position, direction);
    }

}
