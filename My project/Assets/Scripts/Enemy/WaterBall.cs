using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaterBall : IUpdatable
{
    private Transform ballTransform;
    private Transform playerTransform;
    private Vector3 direction;
    private float speed;
    private float lifetime;
    private float timer;
    private bool isActive;

    private GameObject visualObject;
    private Collider[] hitBuffer = new Collider[1]; 

    public WaterBall(Transform transform, float speed, float lifetime)
    {
        this.ballTransform = transform;
        this.speed = speed;
        this.lifetime = lifetime;
        this.visualObject = transform.gameObject;
        Deactivate();
    }

    public void SetPlayer(Transform player)
    {
        playerTransform = player;
    }

    public void Activate(Vector3 position, Vector3 direction)
    {
        this.direction = direction.normalized;
        this.timer = 0f;
        this.isActive = true;
        ballTransform.position = position;
        visualObject.SetActive(true);
    }

    public void Tick(float deltaTime)
    {
        if (!isActive) return;

        ballTransform.position += direction * speed * deltaTime;
        timer += deltaTime;

        
        int hits = Physics.OverlapSphereNonAlloc(ballTransform.position, 0.4f, hitBuffer);

        for (int i = 0; i < hits; i++)
        {
            if (hitBuffer[i].transform == playerTransform)
            {
                Debug.Log("[WaterBall] Impactó al jugador (con OverlapSphere)!");
                Deactivate();
                return;
            }
        }

        if (timer >= lifetime)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        isActive = false;
        visualObject.SetActive(false);
    }

    public bool IsActive() => isActive;
    public Transform GetTransform() => ballTransform;
}
