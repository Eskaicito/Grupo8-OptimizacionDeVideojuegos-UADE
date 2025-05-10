using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : IUpdatable
{
    private Transform transform;
    private Vector3 direction;
    private float speed;
    private float lifetime;
    private float timer;
    private Transform spawner;
    private float fireRate;
    private float fireTimer;

    public BulletLogic(
        Transform bulletTransform,
        Vector3 dir,
        float spd,
        float life,
        Transform spawnerTransform,
        float rate)
    {
        transform = bulletTransform;
        direction = dir.normalized;
        speed = spd;
        lifetime = life;
        spawner = spawnerTransform;
        fireRate = rate;
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
