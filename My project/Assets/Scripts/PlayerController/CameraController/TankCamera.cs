using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : IUpdatable
{

    private Transform cameraTransform;
    private Transform targetTransform;

    private float distance = 9f;
    private float height = 2f;
    private float rotationSpeed = 2000f;
    private float smoothSpeed = 10f;

    private float currentYaw;
    private float currentPitch = 15f;
    private float minPitch = 0f;
    private float maxPitch = 60f;

    public TankCamera(Transform cameraTransform, Transform target)
    {
        this.cameraTransform = cameraTransform;
        this.targetTransform = target;
    }

    public void Tick(float deltaTime)
    {
       
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        currentYaw += mouseX * rotationSpeed * deltaTime;
        currentPitch = Mathf.Clamp(currentPitch + mouseY * rotationSpeed * deltaTime, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = targetTransform.position + Vector3.up * height + offset;

        
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed * deltaTime);
        cameraTransform.LookAt(targetTransform.position + Vector3.up * 1.5f);
    }
}
