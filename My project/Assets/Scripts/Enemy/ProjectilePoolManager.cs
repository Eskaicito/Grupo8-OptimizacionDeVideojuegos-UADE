using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private Transform playerTransform;

    private Queue<WaterBall> pooledProjectiles = new Queue<WaterBall>();
    private CustomUpdateManager updateManager;

    void Awake()
    {
        updateManager = FindFirstObjectByType<CustomUpdateManager>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(projectilePrefab); 
            go.SetActive(false);

            var ball = new WaterBall(go.transform, 10f, 3f);
            ball.SetPlayer(playerTransform);

            pooledProjectiles.Enqueue(ball);
            updateManager.Register(ball);
        }
    }

    public void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        if (pooledProjectiles.Count == 0)
        {
            Debug.LogWarning("¡El pool está vacío! No se pudo disparar.");
            return;
        }

        WaterBall ball = pooledProjectiles.Dequeue();

        if (!ball.IsActive())
        {
            ball.Activate(position, direction);
        }

        pooledProjectiles.Enqueue(ball);
    }
}
