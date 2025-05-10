using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //[SerializeField] private List<GameObject> bulletObjects;

    //private Queue<GameObject> bulletPool = new Queue<GameObject>();

    //void Awake()
    //{
    //    foreach (var bullet in bulletObjects)
    //    {
    //        bullet.SetActive(false);
    //        bulletPool.Enqueue(bullet);
    //    }
    //}

    //public GameObject GetBullet()
    //{
    //    if (bulletPool.Count > 0)
    //    {
    //        var bulletGO = bulletPool.Dequeue();
    //        bulletGO.SetActive(true);
    //        return bulletGO;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("No bullets available in pool!");
    //        return null;
    //    }
    //}

    //public void ReturnBullet(GameObject bulletGO)
    //{
    //    bulletGO.SetActive(false);
    //    bulletPool.Enqueue(bulletGO);
    //}
}
