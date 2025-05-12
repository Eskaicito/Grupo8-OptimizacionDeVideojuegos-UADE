using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObstacle : IUpdatable
{
    private readonly Transform transform;
    private readonly Vector3 startPosition;
    private readonly Vector3 moveDirection;
    private readonly float moveSpeed;
    private readonly float moveRange;
    private readonly float phaseOffset;

    private float moveTimer;

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