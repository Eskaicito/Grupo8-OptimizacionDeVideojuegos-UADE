using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionUtils
{
    // AABB simple: dos cubos definidos por posición centro y tamaño
    public static bool CheckAABBCollision(Vector3 posA, Vector3 sizeA, Vector3 posB, Vector3 sizeB)
    {
        // Convertimos a min y max (esquinas del cubo)
        Vector3 minA = posA - sizeA * 0.5f;
        Vector3 maxA = posA + sizeA * 0.5f;

        Vector3 minB = posB - sizeB * 0.5f;
        Vector3 maxB = posB + sizeB * 0.5f;

        // Comprobamos si se solapan en los 3 ejes
        bool overlapX = maxA.x >= minB.x && minA.x <= maxB.x;
        bool overlapY = maxA.y >= minB.y && minA.y <= maxB.y;
        bool overlapZ = maxA.z >= minB.z && minA.z <= maxB.z;

        return overlapX && overlapY && overlapZ;
    }
}

