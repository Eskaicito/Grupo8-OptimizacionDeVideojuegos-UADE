using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatforms : MonoBehaviour, IUpdatable
{

    private readonly Transform platform;
    private readonly Transform fallingCube;
    private readonly Transform objective;
    private readonly Transform player;

    private readonly float fallingSpeed;
    private readonly Vector2 platformXZ;
    private const float MinHeight = 0.5f;
    private const float MaxHeight = 2f;
    private const float TriggerDistanceSqr = 0.25f;

    private bool isFalling = false;

    public TrapPlatforms(Transform platform, Transform cube, Transform goal, Transform player, float speed)
    {
        this.platform = platform;
        fallingCube = cube;
        objective = goal;
        this.player = player;
        fallingSpeed = speed;

        platformXZ = new Vector2(platform.position.x, platform.position.z);
    }

    public void Tick(float deltaTime)
    {
        if (!isFalling && IsPlayerInArea())
        {
            isFalling = true;
            fallingCube.gameObject.SetActive(true);
        }

        if (isFalling)
        {
            fallingCube.position = Vector3.MoveTowards(fallingCube.position, objective.position, fallingSpeed * deltaTime);
        }
    }

    private bool IsPlayerInArea()
    {
        Vector3 pos = player.position;
        float hDist = (new Vector2(pos.x, pos.z) - platformXZ).sqrMagnitude;
        float vOffset = pos.y - platform.position.y;

        return hDist < TriggerDistanceSqr && vOffset > MinHeight && vOffset < MaxHeight;
    }
}
