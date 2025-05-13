using UnityEngine;

public class PlayerShadow : IUpdatable
{
    private readonly Transform shadowTransform;
    private readonly Transform playerTransform;
    private readonly LayerMask groundMask;
    private static readonly RaycastHit[] hits = new RaycastHit[1];

    private static readonly Quaternion CachedShadowRotation = Quaternion.Euler(90f, 0f, 0f);

    public PlayerShadow(Transform shadowTransform, Transform playerTransform, LayerMask groundMask)
    {
        this.shadowTransform = shadowTransform;
        this.playerTransform = playerTransform;
        this.groundMask = groundMask;
    }

    public void Tick(float deltaTime)
    {
        Vector3 origin = playerTransform.position + Vector3.up * 5f;
        if (Physics.RaycastNonAlloc(origin, Vector3.down, hits, 100f, groundMask) > 0)
        {
            Vector3 hitPoint = hits[0].point;
            shadowTransform.position = new Vector3(playerTransform.position.x, hitPoint.y + 0.05f, playerTransform.position.z);
            shadowTransform.rotation = CachedShadowRotation;
        }
    }
}
