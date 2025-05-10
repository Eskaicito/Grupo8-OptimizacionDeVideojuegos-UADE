using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLogic : MonoBehaviour
{
    //private Transform transform;
    //private BulletPool bulletPool;
    //private CustomUpdateManager updateManager;
    //private Vector3 direction;
    //private float bulletSpeed;
    //private float bulletLifetime;
    //private float fireRate;
    //private float timer;

    //private List<BulletLogic> activeBullets = new List<BulletLogic>();

    //public SpawnerLogic(
    //    Transform t, BulletPool pool, CustomUpdateManager manager,
    //    Vector3 dir, float speed, float lifetime, float rate)
    //{
    //    transform = t;
    //    bulletPool = pool;
    //    updateManager = manager;
    //    direction = dir;
    //    bulletSpeed = speed;
    //    bulletLifetime = lifetime;
    //    fireRate = rate;
    //    timer = 0f;
    //}

    //public void Tick(float deltaTime)
    //{
    //    timer -= deltaTime;

    //    if (timer <= 0f)
    //    {
    //        var bulletGO = bulletPool.GetBullet();
    //        if (bulletGO != null)
    //        {
    //            bulletGO.transform.position = transform.position;
    //            bulletGO.transform.forward = direction;

    //            var bulletLogic = new BulletLogic(
    //                bulletGO.transform, direction, bulletSpeed, bulletLifetime,
    //                bulletGO, bulletPool
    //            );

    //            activeBullets.Add(bulletLogic);
    //            updateManager.Register(bulletLogic);
    //        }

    //        timer = fireRate;
    //    }

    //    // MARCAMOS los muertos, pero NO removemos aquí:
    //    foreach (var bullet in activeBullets)
    //    {
    //        if (bullet.IsDead && !bullet.MarkedForRemoval)
    //        {
    //            bullet.MarkedForRemoval = true;
    //        }
    //    }
    //}

    //// Llamá este método aparte, por ejemplo desde otro lugar, al final del frame:
    //public void Cleanup()
    //{
    //    for (int i = activeBullets.Count - 1; i >= 0; i--)
    //    {
    //        var bullet = activeBullets[i];
    //        if (bullet.MarkedForRemoval)
    //        {
    //            bullet.Kill();
    //            updateManager.Unregister(bullet);
    //            activeBullets.RemoveAt(i);
    //        }
    //    }
    //}
}
