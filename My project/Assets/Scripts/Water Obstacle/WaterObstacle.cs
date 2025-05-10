using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*public class WaterObstacle : IUpdatable
{
    private Transform transform;
    private Vector3 startPosition;
    private Vector3 moveDirection;
    private float moveSpeed;
    private float moveDuration;
    private float cooldownDuration;
    private float timer;
    private bool isMoving;

    private Vector3 boundsSize;
    private PlayerController player;
    private float cooldownTimer;

    public WaterObstacle(Transform transform, Vector3 direction, float speed, float duration, float cooldown, Vector3 boundsSize, PlayerController player)
    {
        this.transform = transform;
        this.startPosition = transform.position;
        this.moveDirection = direction.normalized;
        this.moveSpeed = speed;
        this.moveDuration = duration;
        this.cooldownDuration = cooldown;
        this.boundsSize = boundsSize;
        this.player = player;
        this.timer = 0f;
        this.cooldownTimer = 0f;
        this.isMoving = true;
    }

    public void Tick(float deltaTime)
    {
        if (isMoving)
        {
            timer += deltaTime;
            transform.position += moveDirection * moveSpeed * deltaTime;

            // Check collision
            Vector3 obstaclePos = transform.position;
            Vector3 playerPos = player.GetPosition();
            Vector3 playerSize = player.GetBoundsSize();

            bool collided = CollisionUtils.CheckAABBCollision(obstaclePos, boundsSize, playerPos, playerSize);

            if (collided)
            {
                Vector3 push = moveDirection * 2f; // configurable si querés
                player.ApplyExternalPush(push);
            }

            if (timer >= moveDuration)
            {
                isMoving = false;
                cooldownTimer = 0f;
                transform.position = startPosition;
            }
        }
        else
        {
            cooldownTimer += deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                timer = 0f;
                isMoving = true;
            }
        }
    }

    // Solo si querés ver la caja con Gizmos desde otro lado
    public void DrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boundsSize);
    }
}*/
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