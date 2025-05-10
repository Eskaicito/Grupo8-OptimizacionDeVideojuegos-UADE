using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : IUpdatable
{

    private readonly Transform cameraTransform;
    private readonly Transform targetTransform;

    private float currentYaw = 0f;
    private const float rotationSpeed = 90f; // grados por segundo
    private const float distance = 9f;
    private const float height = 2f;
    private const float smoothSpeed = 10f;

    private static readonly Vector3 OffsetVector = new Vector3(0f, 0f, -distance);
    private static readonly Vector3 LookAtOffset = new Vector3(0f, 1.5f, 0f);

    public TankCamera(Transform cameraTransform, Transform target)
    {
        this.cameraTransform = cameraTransform;
        this.targetTransform = target;
    }

    public void Tick(float deltaTime)
    {
       
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.Q))
            rotationInput = -1f;
        else if (Input.GetKey(KeyCode.E))
            rotationInput = 1f;

        if (rotationInput != 0f)
            currentYaw += rotationInput * rotationSpeed * deltaTime;

        
        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 offset = rotation * OffsetVector;
        Vector3 desiredPosition = targetTransform.position + Vector3.up * height + offset;

        
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed * deltaTime);

        
        cameraTransform.LookAt(targetTransform.position + LookAtOffset);
    }
}
