using UnityEngine;

public static class PhysicsHelper
{
    public static Vector3 ApplyForce(Vector3 position, Vector3 direction, float force, float deltaTime)
    {
        MathHelper.NormalizeSafe(ref direction);
        return position + direction * force * deltaTime;
    }

    public static float ApplyGravity(float velocityY, float gravity, float deltaTime, bool isGrounded)
    {
        if (isGrounded)
        {
            if (velocityY < 0f)
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
        Vector3 verticalMove = new Vector3(0f, velocityY * deltaTime, 0f);
        return position + verticalMove;
    }
}
