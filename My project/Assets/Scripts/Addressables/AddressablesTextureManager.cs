using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesAssetManager : MonoBehaviour
{
    [System.Serializable]
    public class MaterialAssignment
    {
        public string address;                 // Address de Addressables
        public List<Renderer> targetRenderers; // Renderers que recibirán este material
    }

    [System.Serializable]
    public class TextureAssignment
    {
        public string address;                   // Address de Addressables
        public List<Renderer> targetRenderers;   // Renderers que recibirán la textura en el material
        public List<RawImage> targetRawImages;   // RawImages de UI que recibirán la textura
        public List<Image> targetImages;         // Images de UI que recibirán la textura
    }

    [Header("Materials to Load")]
    [SerializeField] private List<MaterialAssignment> materialsToLoad = new List<MaterialAssignment>();

    [Header("Textures to Load")]
    [SerializeField] private List<TextureAssignment> texturesToLoad = new List<TextureAssignment>();

    private List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();

    private void Awake()
    {
        LoadMaterials();
        LoadTextures();
    }

    private void LoadMaterials()
    {
        foreach (var matAssign in materialsToLoad)
        {
            var handle = Addressables.LoadAssetAsync<Material>(matAssign.address);
            handle.Completed += (completedHandle) =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Material loadedMaterial = completedHandle.Result;
                    foreach (var renderer in matAssign.targetRenderers)
                    {
                        if (renderer != null)
                            renderer.sharedMaterial = loadedMaterial; // Usa sharedMaterial para evitar duplicaciones
                    }
                    Debug.Log($"[Addressables] Material '{matAssign.address}' loaded and assigned to {matAssign.targetRenderers.Count} renderers.");
                }
                else
                {
                    Debug.LogError($"[Addressables] Failed to load material: {matAssign.address}");
                }
            };
            handles.Add(handle);
        }
    }

    private void LoadTextures()
    {
        foreach (var texAssign in texturesToLoad)
        {
            var handle = Addressables.LoadAssetAsync<Texture2D>(texAssign.address);
            handle.Completed += (completedHandle) =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Texture2D texture = completedHandle.Result;

                    // Asignar a renderers
                    foreach (var renderer in texAssign.targetRenderers)
                    {
                        if (renderer != null && renderer.sharedMaterial != null)
                            renderer.sharedMaterial.mainTexture = texture;
                    }

                    // Asignar a RawImages
                    foreach (var rawImage in texAssign.targetRawImages)
                    {
                        if (rawImage != null)
                            rawImage.texture = texture;
                    }

                    // Asignar a Images
                    foreach (var image in texAssign.targetImages)
                    {
                        if (image != null)
                        {
                            Sprite sprite = Sprite.Create(
                                texture,
                                new Rect(0, 0, texture.width, texture.height),
                                new Vector2(0.5f, 0.5f));
                            image.sprite = sprite;
                        }
                    }

                    Debug.Log($"[Addressables] Texture '{texAssign.address}' loaded and assigned.");
                }
                else
                {
                    Debug.LogError($"[Addressables] Failed to load texture: {texAssign.address}");
                }
            };
            handles.Add(handle);
        }
    }

    public void ReleaseAssets()
    {
        foreach (var handle in handles)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        handles.Clear();
        Debug.Log("[Addressables] All assets released.");
    }
}
