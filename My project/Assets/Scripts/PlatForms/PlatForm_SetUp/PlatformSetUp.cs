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

    [Header("Trap Platforms")]
    [SerializeField] private List<Transform> trapBases;
    [SerializeField] private List<Transform> fallingCubes;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private float fallingSpeed = 200f;

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

        for (int i = 0; i < trapBases.Count; i++)
        {
            var logic = new TrapPlatforms(
                trapBases[i],
                fallingCubes[i],
                targets[i],
                player,
                fallingSpeed
            );
            updateManager.Register(logic);
        }
    }
}
