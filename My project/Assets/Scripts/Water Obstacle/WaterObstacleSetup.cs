using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Clase SetUp de los obstaculos de agua
// Este script es para la lógica de los obstáculos de agua en el juego.
// Se encarga de inicializar los obstáculos de agua y registrarlos en el CustomUpdateManager.
// Se utiliza la clase CustomUpdateManager para registrar la lógica de los obstáculos de agua y actualizarlas en cada frame.
public class WaterObstacleSetup : MonoBehaviour
{
    [SerializeField] private Transform obstaclesRightRoot;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float range = 3f;

    private static readonly Vector3 RightDirection = new Vector3(0f, 0f, 1f);

    private void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();

        
        RegisterObstacles(obstaclesRightRoot, RightDirection, updateManager);
    }

    // Este metodo se encarga de registrar los obstaculos de agua en el CustomUpdateManager

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