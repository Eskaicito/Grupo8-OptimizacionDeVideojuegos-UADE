using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugGizmos : MonoBehaviour
{
    public Vector3 size = Vector3.one;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
