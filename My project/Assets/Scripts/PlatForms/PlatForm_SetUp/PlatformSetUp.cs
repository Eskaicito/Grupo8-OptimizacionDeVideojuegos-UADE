using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSetUp : MonoBehaviour
{
    [SerializeField] private CustomUpdateManager updateManager;
    [SerializeField] private Transform player;

    [Header("Ghost Platforms")]
    [SerializeField] private List<Renderer> ghostRenderers;
    [SerializeField] private List<Collider> ghostColliders;
    [SerializeField] private float intervalMin = 3f;
    [SerializeField] private float intervalMax = 5f;
    [SerializeField] private float warningTime = 1f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;

    void Awake()
    {
        for (int i = 0; i < ghostRenderers.Count; i++)
        {
            var logic = new GhostPlatforms(
                ghostRenderers[i],
                ghostColliders[i],
                intervalMin,
                intervalMax,
                warningTime,
                normalColor,
                warningColor
            );
            updateManager.Register(logic);
        }

    }
}
