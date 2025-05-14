using UnityEngine;


/// <summary>
/// Cámara estilo tanque.
/// Permite rotar alrededor del jugador utilizando teclas Q y E.
/// Realiza una corrección de colisión mediante SphereCast para evitar que la cámara atraviese objetos.
/// Se ejecuta mediante IUpdatable
/// </summary>
public class TankCamera : IUpdatable
{
    private readonly Transform cameraTransform;
    private readonly Transform targetTransform;

    private float currentYaw = 0f;
    private float lastYaw = float.MinValue;
    private Vector3 forward = Vector3.forward;

    private readonly float rotationSpeed = 90f;
    private readonly float distance = 9f;
    private readonly float height = 2f;
    private readonly float smoothSpeed = 10f;
    private readonly float minDistance = 1.5f;
    private readonly float sphereRadius = 0.3f;
    private readonly float Deg2Rad = Mathf.PI / 180f;

    private readonly LayerMask groundMask;
    private readonly RaycastHit[] _cameraHits = new RaycastHit[1];

    private static readonly Vector3 LookAtOffset = new Vector3(0f, 1.5f, 0f);

    // Rotación fija vertical (pitch) para inclinar la cámara hacia abajo
    private static readonly Quaternion PitchRotation = Quaternion.Euler(25f, 0f, 0f);

    /// <summary>
    /// Constructor. Recibe todas las dependencias necesarias.
    /// </summary>
    public TankCamera(Transform cameraTransform, Transform target, LayerMask groundMask)
    {
        this.cameraTransform = cameraTransform;
        this.targetTransform = target;
        this.groundMask = groundMask;
    }
    /// <summary>
    /// Tick que gestiona el input, rotación, corrección de colisiones y posicionamiento de la cámara.
    /// </summary>
    public void Tick(float deltaTime)
    {
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.Q)) rotationInput = -1f;
        else if (Input.GetKey(KeyCode.E)) rotationInput = 1f;

        if (rotationInput != 0f)
            currentYaw += rotationInput * rotationSpeed * deltaTime;

        if (currentYaw != lastYaw)
        {
            float yawRad = currentYaw * Deg2Rad;
            forward.Set(Mathf.Sin(yawRad), 0f, Mathf.Cos(yawRad));
            lastYaw = currentYaw;
        }

 
        Vector3 offset = PitchRotation * (forward * -distance);

        Vector3 targetHead = targetTransform.position + Vector3.up * height;
        float desiredDistance = offset.magnitude;
        Vector3 desiredDirection = offset / desiredDistance;

        Vector3 finalPosition;

        int hitCount = Physics.SphereCastNonAlloc(
            targetHead,
            sphereRadius,
            desiredDirection,
            _cameraHits,
            desiredDistance,
            groundMask
        );

        if (hitCount > 0)
        {
            float correctedDistance = Mathf.Max(minDistance, _cameraHits[0].distance - 0.1f);
            finalPosition = targetHead + desiredDirection * correctedDistance;
        }
        else
        {
            finalPosition = targetHead + offset;
        }

        if ((cameraTransform.position - finalPosition).sqrMagnitude > 0.0001f)
        {
            cameraTransform.position = Vector3.Lerp(
                cameraTransform.position,
                finalPosition,
                smoothSpeed * deltaTime
            );
        }

        cameraTransform.LookAt(targetTransform.position + LookAtOffset);
    }
}
