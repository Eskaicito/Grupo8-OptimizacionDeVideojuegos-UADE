using UnityEngine;

public static class PhysicsHelper
{
    public static Vector3 CalculateExternalPush(Vector3 currentPush, Vector3 direction, float force)
    {
        MathHelper.NormalizeSafe(ref direction);
        return currentPush + direction * force;
    }

    public static float ApplyGravity(float velocityY, float gravity, float deltaTime, bool isGrounded)
    {
        if (isGrounded && velocityY < 0f)
        {
            velocityY = 0f;
        }
        else
        {
            velocityY += gravity * deltaTime;
        }
        return velocityY;
    }

    public static Vector3 ApplyVerticalMovement(Vector3 position, float velocityY, float deltaTime)
    {
        return position + new Vector3(0f, velocityY * deltaTime, 0f);
    }
}
