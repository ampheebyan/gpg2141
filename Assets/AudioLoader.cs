using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public enum Type
    {
        file,
        bundle
    }
    
    public Type type = Type.file;

    [SerializeField] private AudioSource src;
    
    [SerializeField] private int sampleRate = 44100;
    [SerializeField] private string nameInBundle = "audio";
    [SerializeField] private string filePath = "";

    [SerializeField] private bool loadOnEnable = false;

    private void OnEnable()
    {
        if (loadOnEnable)
        {
            switch (type)
            {
                case Type.file:
                    LoadFromFile(filePath, sampleRate);
                    break;
                case Type.bundle:
                    LoadFromBundle(nameInBundle, filePath);
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            src.Play();
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
            AssetBundle bundle = AssetBundle.LoadFromFile(filePath);

            if (bundle == null)
            {
                Debug.LogError("Bundle file is empty or failed to load.");
                return;
            }

            AudioClip asset = bundle.LoadAsset<AudioClip>(assetName);

            if (asset)
            {
                src.clip = asset;
            } 
        }
    }
    
    public void LoadFromFile(string path = "nothing", int sampleRate = 44100)
    {
        if (type != Type.file) return;
        if (path == "nothing") path = filePath;
        path = Path.Combine(Application.streamingAssetsPath, path);

        if (FSOperations.FileExists(path))
        {
            byte[] raw = File.ReadAllBytes(path);
            if (raw.Length < 1)
                return;
            
            float[] data = new float[raw.Length / 2];
            if (data.Length < 1)
                return;
            
            for (int i = 0; i < data.Length; i++)
            {
                short bit = BitConverter.ToInt16(raw, i * 2);
                data[i] = bit / (float)short.MaxValue;
            }
            
            var clip = AudioClip.Create("_LoadedClip", data.Length, 1, sampleRate, false);
            clip.SetData(data, 0);
            src.clip = clip;
        }
        else
        {
            Debug.LogError($"Path '{filePath}' does not exist or is folder.");
        }
    }

}
