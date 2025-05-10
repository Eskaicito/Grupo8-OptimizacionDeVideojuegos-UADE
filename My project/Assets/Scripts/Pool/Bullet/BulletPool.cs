using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 50;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private CustomUpdateManager updateManager;

    void Awake()
    {
        updateManager = FindFirstObjectByType<CustomUpdateManager>();

        for (int i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public void SpawnBullet(Vector3 position, Vector3 direction, float speed, float lifetime)
    {
        if (bulletPool.Count > 0)
        {
            var bulletGO = bulletPool.Dequeue();
            bulletGO.transform.position = position;
            bulletGO.SetActive(true);

            var bulletLogic = new BulletLogic(
                bulletGO.transform, direction, speed, lifetime,
                bulletGO, this, updateManager
            );

            updateManager.Register(bulletLogic);
        }
    }

    public void ReturnBullet(GameObject bulletGO)
    {
        bulletGO.SetActive(false);
        bulletPool.Enqueue(bulletGO);
    }
}
