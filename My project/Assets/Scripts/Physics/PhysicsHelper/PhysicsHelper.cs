using UnityEngine;

public static class PhysicsHelper
{
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
        return position + Vector3.up * velocityY * deltaTime;
    }

    public static Vector3 CalculateImpulsePush(Vector3 direction, float force, Vector3 currentMomentum)
    {
        MathHelper.NormalizeSafe(ref direction);
        float alignment = Vector3.Dot(currentMomentum, direction);

       
        if (alignment > 0.5f)
        {
            force *= 0.5f;
        }

        return direction * force;
    }
}
