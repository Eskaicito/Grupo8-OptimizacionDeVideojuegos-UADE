using UnityEngine;

public class SceneTriggerLoaderSetup : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private string sceneToLoad = "Level1";

    private void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        var logic = new SceneTriggerLoader(playerTransform, triggerCollider, sceneToLoad);
        updateManager.Register(logic);
    }
}
