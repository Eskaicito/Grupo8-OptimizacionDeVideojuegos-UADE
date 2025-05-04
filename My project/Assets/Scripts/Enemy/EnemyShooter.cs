using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShooter : IUpdatable
{
    private Transform enemyTransform;
    private float fireRate;
    private float fireCooldown;

    public delegate void OnFire(Vector3 position, Vector3 direction);
    private OnFire fireCallback;

    public EnemyShooter(Transform transform, float fireRate, OnFire fireCallback)
    {
        this.enemyTransform = transform;
        this.fireRate = fireRate;
        this.fireCallback = fireCallback;
        this.fireCooldown = fireRate;
    }

    public void Tick(float deltaTime)
    {
        fireCooldown -= deltaTime;

        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = fireRate;
        }
    }

    private void Fire()
    {
        
        Vector3 direction = enemyTransform.forward;

        
        Vector3 spawnPosition = enemyTransform.position + direction * 0.5f;

        
        fireCallback?.Invoke(spawnPosition, direction);

        
        Debug.Log($"[EnemyShooter] Disparo desde {spawnPosition} hacia {direction}");
    }
}

