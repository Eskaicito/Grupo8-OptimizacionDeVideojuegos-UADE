using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : IUpdatable
{
    private readonly Transform transform;
    private readonly Transform spawner;
    private readonly Vector3 direction;
    private readonly float speed;
    private readonly float lifetime;
    private readonly float fireRate;

    private float timer;
    private float fireTimer;

    public BulletLogic(
        Transform bulletTransform,
        Vector3 direction,
        float speed,
        float lifetime,
        Transform spawnerTransform,
        float fireRate)
    {
        this.transform = bulletTransform;
        this.direction = direction.normalized;
        this.speed = speed;
        this.lifetime = lifetime;
        this.spawner = spawnerTransform;
        this.fireRate = fireRate;

        timer = lifetime; 
        fireTimer = 0f;
    }

    public void Tick(float deltaTime)
    {
        fireTimer -= deltaTime;

        if (fireTimer <= 0f)
        {
            transform.position = spawner.position;
            timer = 0f;
            fireTimer = fireRate;
        }

        if (timer < lifetime)
        {
            transform.position += direction * speed * deltaTime;
            timer += deltaTime;
        }
    }
}
