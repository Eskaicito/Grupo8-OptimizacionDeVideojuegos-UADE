using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WinZone : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private BoxCollider boxCollider;
    private Collider[] buffer = new Collider[1];

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("WinZone necesita un BoxCollider.");
            return;
        }

        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        updateManager.Register(new WinZoneLogic(boxCollider, playerTransform, buffer));
    }

    private class WinZoneLogic : IUpdatable
    {
        private readonly BoxCollider boxCollider;
        private readonly Transform playerTransform;
        private readonly Collider[] buffer;

        public WinZoneLogic(BoxCollider boxCollider, Transform playerTransform, Collider[] buffer)
        {
            this.boxCollider = boxCollider;
            this.playerTransform = playerTransform;
            this.buffer = buffer;
        }

        public void Tick(float deltaTime)
        {
            Vector3 center = boxCollider.bounds.center;
            Vector3 halfExtents = boxCollider.bounds.extents;
            Quaternion rotation = boxCollider.transform.rotation;

            int hits = Physics.OverlapBoxNonAlloc(center, halfExtents, buffer, rotation);
            for (int i = 0; i < hits; i++)
            {
                if (buffer[i] != null && buffer[i].transform == playerTransform)
                {
                    Debug.Log("¡Victoria! El jugador tocó la zona.");
                    Application.Quit();
                }
            }
        }
    }
}
