using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*public class WaterObstacleSetup : MonoBehaviour
{
    public Transform obstacleTransform;
    public Transform playerTransform;
    CustomUpdateManager customUpdateManager;

    public Vector3 moveDirection = Vector3.forward;
    public float speed = 2f;
    public float moveDuration = 3f;
    public float cooldown = 2f;
    public Vector3 boundsSize = new Vector3(2f, 1f, 1f);
    public Vector3 playerSize = new Vector3(1f, 1f, 1f);

    void Awake()
    {
        var playerController = new PlayerController(playerTransform, playerSize);
        var waterObstacle = new WaterObstacle(obstacleTransform, moveDirection, speed, moveDuration, cooldown, boundsSize, playerController);
        if (customUpdateManager != null) { Debug.Log("no esta el custom manager"); }
        customUpdateManager.Register(waterObstacle);
        //customUpdateManager.Register(playerController);
    }
}*/
public class WaterObstacleSetup : MonoBehaviour
{
    public Transform obstaclesLeftRoot;
    public Transform obstaclesRightRoot;

    public float speed = 1f;
    public float range = 3f;

    void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();

        foreach (Transform child in obstaclesLeftRoot)
        {
            float offset = Random.Range(0f, Mathf.PI * 2f); // desfase entre 0 y 360°
            var logic = new WaterObstacle(child, Vector3.right, speed, range, offset);
            updateManager.Register(logic);
        }

        foreach (Transform child in obstaclesRightRoot)
        {
            float offset = Random.Range(0f, Mathf.PI * 2f);
            var logic = new WaterObstacle(child, Vector3.left, speed, range, offset);
            updateManager.Register(logic);
        }
    }
}