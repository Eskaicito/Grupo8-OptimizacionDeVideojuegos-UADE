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
        public string address;                 
        public List<Renderer> targetRenderers; 
    }

    [System.Serializable]
    public class TextureAssignment
    {
        public string address;                 
        public List<Renderer> targetRenderers;   
        public List<RawImage> targetRawImages;   
        public List<Image> targetImages;         
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
                            renderer.sharedMaterial = loadedMaterial; 
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
            var handle = Addressables.LoadAssetAsync<Sprite>(texAssign.address);
            handle.Completed += (completedHandle) =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Sprite sprite = completedHandle.Result;
                    foreach (var image in texAssign.targetImages)
                    {
                        if (image != null)
                            image.sprite = sprite;
                    }
                    Debug.Log($"[Addressables] Sprite '{texAssign.address}' loaded and assigned.");
                }
                else
                {
                    Debug.LogError($"[Addressables] Failed to load sprite: {texAssign.address}");
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
