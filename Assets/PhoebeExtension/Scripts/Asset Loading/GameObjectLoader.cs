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
        // check if loadonenable
        if (loadOnEnable)
        {
            // check what type
            switch (type)
            {
                // trigger load from bundle
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
        // validation
        if (this.type != Type.bundle) return;
        if (path == "nothing") path = filePath;
        path = Path.Combine(Application.streamingAssetsPath, path);

        // check if file exists
        if (FSOperations.FileExists(path))
        {
            // load bundle
            AssetBundle bundle = AssetBundle.LoadFromFile(path);

            // check if bundle actually loaded
            if (bundle == null)
            {
                Debug.LogError("Bundle file is empty or failed to load.");
                return;
            }
    
            // grab asset
            GameObject asset = bundle.LoadAsset<GameObject>(assetName);
            // check if asset was actually grabbed
            if (asset)
            {
                // clear possible parents
                foreach (Transform _ in parents)
                {
                    foreach(Transform child in _)
                        Destroy(child.gameObject);
                    // instantiate with _ as parent
                    GameObject go = GameObject.Instantiate(asset, _);
                }
            }
        }
    }
}
