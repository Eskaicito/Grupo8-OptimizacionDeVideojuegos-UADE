using UnityEngine;

public class CollisionManager : IUpdatable
{
    private readonly Transform trackedTransform;
    private readonly LayerMask groundMask;
    private readonly LayerMask wallMask;
    private readonly LayerMask obstacleMask;
    private readonly LayerMask bulletMask;
    private readonly LayerMask winZoneMask;

    private static readonly RaycastHit[] hitResults = new RaycastHit[1];
    private static readonly Vector3 UpOffset = new Vector3(0f, 0.1f, 0f);

    public bool IsGrounded { get; private set; }
    public bool IsTouchingWall { get; private set; }
    public bool IsTouchingObstacle { get; private set; }
    public bool IsTouchingBullet { get; private set; }
    public bool IsInWinZone { get; private set; }

    public Vector3 LastObstacleDirection { get; private set; }
    public Vector3 LastBulletDirection { get; private set; }
    public Vector3 GroundNormal { get; private set; } = Vector3.up;

    public CollisionManager(
        Transform trackedTransform,
        LayerMask groundMask,
        LayerMask wallMask,
        LayerMask obstacleMask,
        LayerMask bulletMask,
        LayerMask winZoneMask)
    {
        this.trackedTransform = trackedTransform;
        this.groundMask = groundMask;
        this.wallMask = wallMask;
        this.obstacleMask = obstacleMask;
        this.bulletMask = bulletMask;
        this.winZoneMask = winZoneMask;
    }

    public void Tick(float deltaTime)
    {
        Vector3 origin = trackedTransform.position + UpOffset;
        Vector3 forward = trackedTransform.forward;

        IsGrounded = SphereCastWithNormal(origin, Vector3.down, 0.3f, 0.5f, groundMask, out Vector3 normal);
        GroundNormal = normal;

        IsTouchingWall = SphereCast(origin, forward.normalized, 0.3f, 0.2f, wallMask);
        IsTouchingObstacle = SphereCast(origin, forward, 0.3f, 0.5f, obstacleMask, out Vector3 obstacleDir);
        LastObstacleDirection = obstacleDir;

        IsTouchingBullet = SphereCast(origin, forward, 0.3f, 0.5f, bulletMask, out Vector3 bulletDir);
        LastBulletDirection = bulletDir;

        IsInWinZone = SphereCast(origin, Vector3.forward, 0.3f, 0.5f, winZoneMask);
    }

    public bool CheckWall(Vector3 direction)
    {
        Vector3 origin = trackedTransform.position + UpOffset;
        return SphereCast(origin, direction.normalized, 0.3f, 0.2f, wallMask);
    }

    public float GetGroundPenetration()
    {
        Vector3 origin = trackedTransform.position + UpOffset;
        if (Physics.SphereCastNonAlloc(origin, 0.3f, Vector3.down, hitResults, 0.5f, groundMask) > 0)
        {
            float penetration = 0.3f - hitResults[0].distance;
            return penetration > 0f ? penetration : 0f;
        }
        return 0f;
    }

    public float CorrectCameraDistance(Vector3 origin, Vector3 direction, float desiredDistance, float minDistance, float sphereRadius)
    {
        int hitCount = Physics.SphereCastNonAlloc(origin, sphereRadius, direction, hitResults, desiredDistance, groundMask);
        if (hitCount > 0)
        {
            return Mathf.Max(minDistance, hitResults[0].distance - 0.1f);
        }
        return desiredDistance;
    }

    private static bool SphereCast(Vector3 origin, Vector3 direction, float radius, float distance, LayerMask layerMask)
    {
        return Physics.SphereCastNonAlloc(origin, radius, direction, hitResults, distance, layerMask) > 0;
    }

    private static bool SphereCast(Vector3 origin, Vector3 direction, float radius, float distance, LayerMask layerMask, out Vector3 impactDirection)
    {
        bool hit = Physics.SphereCastNonAlloc(origin, radius, direction, hitResults, distance, layerMask) > 0;
        impactDirection = hit ? -direction : Vector3.zero;
        return hit;
    }

    private static bool SphereCastWithNormal(Vector3 origin, Vector3 direction, float radius, float distance, LayerMask mask, out Vector3 normal)
    {
        if (Physics.SphereCastNonAlloc(origin, radius, direction, hitResults, distance, mask) > 0)
        {
            normal = hitResults[0].normal;
            return true;
        }
        normal = Vector3.up;
        return false;
    }
}
