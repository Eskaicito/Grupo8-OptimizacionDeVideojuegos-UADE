using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesTextureManager : MonoBehaviour
{
    public enum TargetType { Renderer, RawImage, Image }

    [System.Serializable]
    public class TextureAssignment
    {
        public string address;
        public TargetType targetType;
        public Renderer targetRenderer;
        public RawImage targetRawImage;
        public Image targetImage;
    }

    [SerializeField] private List<TextureAssignment> texturesToLoad = new List<TextureAssignment>();

    private List<AsyncOperationHandle<Texture2D>> handles = new List<AsyncOperationHandle<Texture2D>>();

    private void Awake()
    {
        LoadTextures();
    }

    public void LoadTextures()
    {
        foreach (var texAssign in texturesToLoad)
        {
            var handle = Addressables.LoadAssetAsync<Texture2D>(texAssign.address);
            handle.Completed += (completedHandle) =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Texture2D texture = completedHandle.Result;
                    switch (texAssign.targetType)
                    {
                        case TargetType.Renderer:
                            if (texAssign.targetRenderer != null)
                                texAssign.targetRenderer.material.mainTexture = texture;
                            break;
                        case TargetType.RawImage:
                            if (texAssign.targetRawImage != null)
                                texAssign.targetRawImage.texture = texture;
                            break;
                        case TargetType.Image:
                            if (texAssign.targetImage != null)
                            {
                                Sprite sprite = Sprite.Create(
                                    texture,
                                    new Rect(0, 0, texture.width, texture.height),
                                    new Vector2(0.5f, 0.5f));
                                texAssign.targetImage.sprite = sprite;
                            }
                            break;
                    }
                    Debug.Log($"[Addressables] Texture '{texAssign.address}' loaded.");
                }
                else
                {
                    Debug.LogError($"[Addressables] Failed to load texture: {texAssign.address}");
                }
            };
            handles.Add(handle);
        }
    }

    public void ReleaseTextures()
    {
        foreach (var handle in handles)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        handles.Clear();
        Debug.Log("[Addressables] All textures released.");
    }
}
