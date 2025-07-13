using UnityEngine;

public class TankCamera : IUpdatable
{
    private readonly Transform cameraTransform;
    private readonly Transform targetTransform;
    private readonly CollisionManager collisionManager;
    private readonly InputManager inputHandler;

    private float currentYaw = 0f;
    private Vector3 currentDirection = Vector3.back;

    private readonly float orbitSpeed = 120f;
    private readonly float followDistance = 8f;
    private readonly float heightOffset = 2f;
    private readonly float smoothFollow = 10f;
    private readonly float minDistance = 1.5f;
    private readonly float sphereRadius = 0.3f;

    private static readonly Vector3 LookAtOffset = new Vector3(0f, 1.5f, 0f);

    public TankCamera(Transform cameraTransform, Transform targetTransform, CollisionManager collisionManager, InputManager inputHandler)
    {
        this.cameraTransform = cameraTransform;
        this.targetTransform = targetTransform;
        this.collisionManager = collisionManager;
        this.inputHandler = inputHandler;
    }

    public void Tick(float deltaTime)
    {
        Vector3 targetPos = targetTransform.position;
        Vector3 targetHead = targetPos + Vector3.up * heightOffset;

        HandleRotation(deltaTime);
        Vector3 desiredDir = CalculateDesiredDirection();

        float correctedDistance = collisionManager.CorrectCameraDistance(targetHead, desiredDir, followDistance, minDistance, sphereRadius);
        Vector3 desiredPosition = targetHead + desiredDir * correctedDistance;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothFollow * deltaTime);

        if ((currentDirection - desiredDir).sqrMagnitude > 0.0001f)
        {
            currentDirection = Vector3.Slerp(currentDirection, desiredDir, smoothFollow * deltaTime);
        }

        cameraTransform.LookAt(targetPos + LookAtOffset);
    }

    private void HandleRotation(float deltaTime)
    {
        float input = inputHandler.CameraYawInput;
        if (Mathf.Abs(input) > 0.01f)
        {
            currentYaw += input * orbitSpeed * deltaTime;
        }
    }

    private Vector3 CalculateDesiredDirection()
    {
        float yawRad = currentYaw * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(yawRad), 0f, Mathf.Cos(yawRad)) * -1f;
    }
}
