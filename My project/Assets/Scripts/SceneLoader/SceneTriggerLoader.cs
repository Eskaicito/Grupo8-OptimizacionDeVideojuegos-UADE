using UnityEngine;

public class SceneTriggerLoader : IUpdatable
{
    private readonly Transform playerTransform;
    private readonly Collider triggerCollider;
    private readonly string sceneToLoad;
    private bool hasTriggered = false;

    public SceneTriggerLoader(Transform playerTransform, Collider triggerCollider, string sceneToLoad)
    {
        this.playerTransform = playerTransform;
        this.triggerCollider = triggerCollider;
        this.sceneToLoad = sceneToLoad;
    }

    public void Tick(float deltaTime)
    {
        if (hasTriggered)
            return;

        if (triggerCollider.bounds.Intersects(playerTransform.GetComponent<Collider>().bounds))
        {
            hasTriggered = true;
            LoadingData.SceneToLoad = sceneToLoad;
            UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen");
        }
    }
}
