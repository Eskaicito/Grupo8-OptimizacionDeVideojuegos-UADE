using UnityEngine;

public class BasicPostProcessSetup : MonoBehaviour
{
    [SerializeField] private Shader shader;

    private BasicPostProcessLogic logic;

    private void Awake()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        var cam = GetComponent<Camera>();

        logic = new BasicPostProcessLogic(cam, shader);
        updateManager.Register(logic);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        logic.RenderPostProcess(source, destination);
    }

    private void OnDestroy()
    {
        var updateManager = FindFirstObjectByType<CustomUpdateManager>();
        if (updateManager != null)
        {
            updateManager.Unregister(logic);
        }
        logic.Dispose(); // Liberate resources
    }

}
