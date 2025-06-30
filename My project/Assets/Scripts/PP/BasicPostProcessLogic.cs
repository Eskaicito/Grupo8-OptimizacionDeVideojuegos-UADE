using UnityEngine;

public class BasicPostProcessLogic : IUpdatable
{
    private readonly Material mat;
    private readonly Camera camera;

    public BasicPostProcessLogic(Camera camera, Shader shader)
    {
        this.camera = camera;
        mat = new Material(shader);
    }

    public void Tick(float deltaTime)
    {
        
    }

    public void RenderPostProcess(RenderTexture source, RenderTexture destination)
    {
        source.wrapMode = TextureWrapMode.Mirror;
        Graphics.Blit(source, destination, mat);
    }

    public void Dispose()
    {
        if (mat != null)
        {
            Object.Destroy(mat);
        }
    }
}
