using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class WaterObstacleSetup : MonoBehaviour
{
    //[SerializeField] private Transform obstaclesLeftRoot;
    [SerializeField] private Transform obstaclesRightRoot;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float range = 3f;

    //private static readonly Vector3 LeftDirection = new Vector3(0f, 0f, -1f);
    private static readonly Vector3 RightDirection = new Vector3(0f, 0f, 1f);

    private void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();

        //RegisterObstacles(obstaclesLeftRoot, LeftDirection, updateManager);
        RegisterObstacles(obstaclesRightRoot, RightDirection, updateManager);
    }

    private void RegisterObstacles(Transform root, Vector3 direction, CustomUpdateManager updateManager)
    {
        int count = root.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform obstacle = root.GetChild(i);
            float offset = Random.Range(0f, Mathf.PI * 2f);

            var logic = new WaterObstacle(obstacle, direction, speed, range, offset);
            updateManager.Register(logic);
        }
    }
}