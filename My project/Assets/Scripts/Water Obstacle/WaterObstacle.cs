using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script es para la lógica de los obstáculos de agua en el juego.
// Se encarga de mover los obstáculos de agua en una dirección específica y con una velocidad determinada.
// Se utiliza la interfaz IUpdatable para actualizar la lógica de los obstáculos de agua en cada frame.
public class WaterObstacle : IUpdatable
{
    private readonly Transform transform;
    private readonly Vector3 startPosition;
    private readonly Vector3 moveDirection;
    private readonly float moveSpeed;
    private readonly float moveRange;
    private readonly float phaseOffset;

    private float moveTimer;

    // Constructor de la clase WaterObstacle
    // Se inicializan las variables del obstáculo de agua, como la posición inicial, dirección de movimiento, velocidad, rango y desfase.
    // El desfase se utiliza para crear un movimiento oscilante en el obstáculo de agua.
    public WaterObstacle(Transform transform, Vector3 direction, float speed, float range, float offset)
    {
        this.transform = transform;
        this.startPosition = transform.position;
        this.moveDirection = direction.normalized;
        this.moveSpeed = speed;
        this.moveRange = range;
        this.phaseOffset = offset;
        this.moveTimer = 0f;
    }

    public void Tick(float deltaTime)
    {
        moveTimer += deltaTime;

       
        float offset = Mathf.Sin(moveTimer * moveSpeed + phaseOffset) * moveRange;
        Vector3 newPos = startPosition + moveDirection * offset;

        transform.position = newPos;
    }
}