using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : IUpdatable
{

    private readonly Transform cameraTransform;
    private readonly Transform targetTransform;

    private float currentYaw = 0f;
    private float lastYaw = float.MinValue;
    private Vector3 forward = Vector3.forward;

    private const float rotationSpeed = 90f; // grados por segundo
    private const float distance = 9f;
    private const float height = 2f;
    private const float smoothSpeed = 10f;
    private const float minDistance = 1.5f;
    private const float sphereRadius = 0.3f;
    private const float Deg2Rad = Mathf.PI / 180f;

    private static readonly Vector3 LookAtOffset = new Vector3(0f, 1.5f, 0f);

    private readonly LayerMask wallMask;
    private readonly RaycastHit[] _cameraHits = new RaycastHit[1];

    public TankCamera(Transform cameraTransform, Transform target)
    {
        this.cameraTransform = cameraTransform;
        this.targetTransform = target;

        wallMask = LayerMask.GetMask("Wall");
    }

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
            forward = new Vector3(Mathf.Sin(yawRad), 0f, Mathf.Cos(yawRad));
            lastYaw = currentYaw;
        }

        Vector3 offset = forward * -distance;
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
            wallMask
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
