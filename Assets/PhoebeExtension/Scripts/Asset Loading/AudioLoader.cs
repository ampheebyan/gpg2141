using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    // where to load from
    public enum Type
    {
        file,
        bundle
    }
    
    public Type type = Type.file;

    // audiosource to put clip into
    [SerializeField] private AudioSource src;
    
    // samplerate of audio if loaded from file
    [SerializeField] private int sampleRate = 44100;
    // name in bundle
    [SerializeField] private string nameInBundle = "audio";
    // filepath of bundle/asset
    [SerializeField] private string filePath = "";

    // should load on enable?
    [SerializeField] private bool loadOnEnable = false;

    private void OnEnable()
    {
        // check if loadonenable
        if (loadOnEnable)
        {
            // check what type
            switch (type)
            {
                // run through functions
                case Type.file:
                    LoadFromFile(filePath, sampleRate);
                    break;
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
            AssetBundle bundle = AssetBundle.LoadFromFile(filePath);

            // ensure bundle actually loaded
            if (bundle == null)
            {
                Debug.LogError("Bundle file is empty or failed to load.");
                return;
            }

            // load clip
            AudioClip asset = bundle.LoadAsset<AudioClip>(assetName);

            // validate if clip loaded
            if (asset)
            {
                // set audiosrc clip to new clip
                src.clip = asset;
            } 
        }
    }
    
    public void LoadFromFile(string path = "nothing", int sampleRate = 44100)
    {
        // validation        
        if (type != Type.file) return;
        if (path == "nothing") path = filePath;
        path = Path.Combine(Application.streamingAssetsPath, path);

        // check if file exists
        if (FSOperations.FileExists(path))
        {
            // load file raw as bytes
            byte[] raw = File.ReadAllBytes(path);
            if (raw.Length < 1)
                return;
            // stop if byte length is less than 1. probably didnt load properly if that's the case. or it's malformed. i dont care. it didn't load.
            
            // convert to float array and divide by 2 because 2bits = 1byte
            float[] data = new float[raw.Length / 2];
            if (data.Length < 1)
                return;
            // if somehow we didnt stop before, we are now. it didn't load.
            
            for (int i = 0; i < data.Length; i++) // iterate through the float array
            {
                short bit = BitConverter.ToInt16(raw, i * 2); // convert data to 16bit integer, move offset
                data[i] = bit / (float)short.MaxValue; // normalize value
            }
            
            // create clip
            var clip = AudioClip.Create("_LoadedClip", data.Length, 1, sampleRate, false);
            // set data
            clip.SetData(data, 0);
            // set audiosrc clip to new clip
            src.clip = clip;
        }
        else
        {
            Debug.LogError($"Path '{filePath}' does not exist or is folder.");
        }
    }

}
