using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameObjectLoader : MonoBehaviour
{
public enum Type
    {
        bundle
    }
    
    public Type type = Type.bundle;

    [SerializeField] private string nameInBundle = "object";
    [SerializeField] private string filePath = "";

    [SerializeField] private bool loadOnEnable = false;

    [SerializeField] private Transform[] parents;
    private void OnEnable()
    {
        if (loadOnEnable)
        {
            switch (type)
            {
                case Type.bundle:
                    LoadFromBundle(nameInBundle, filePath);
                    break;
            }
        }
    }

    public void SetNewFilePath(string path)
    {
        if (FSOperations.FileExists(path))
        {
            filePath = path;
        }
        else
        {
            Debug.LogError("File does not exist.");
        }
    }

    public void LoadFromBundle(string assetName, string path = "nothing")
    {
        if (this.type != Type.bundle) return;
        if (path == "nothing") path = filePath;
        path = Path.Combine(Application.streamingAssetsPath, path);

        if (FSOperations.FileExists(path))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(path);

            if (bundle == null)
            {
                Debug.LogError("Bundle file is empty or failed to load.");
                return;
            }

            GameObject asset = bundle.LoadAsset<GameObject>(assetName);
            if (asset)
            {
                foreach (Transform _ in parents)
                {
                    foreach(Transform child in _)
                        Destroy(child.gameObject);
                    GameObject go = GameObject.Instantiate(asset, _);
                }
            }
        }
    }
}
