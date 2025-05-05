using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObstacleDebugGizmos : MonoBehaviour
{
    public Vector3 boundsSize = new Vector3(2, 1, 1);

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boundsSize);
    }
}
