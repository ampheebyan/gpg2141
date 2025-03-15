using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class JSONHandler : MonoBehaviour
{
    public static JSONHandler Instance; // instance

    [SerializeField] private string path; // path
    
    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(string.IsNullOrEmpty(path)) path = Application.persistentDataPath; // go to data path if empty
    }

    // save data to JSON, log if success
    public void Save(string fileName, object data, bool overwrite = false)
    {
        string json = JsonUtility.ToJson(data, true);
        bool outcome = FSOperations.WriteFile(Path.Join(path, fileName), json, overwrite);
        Debug.Log(outcome);
    }

    // load as PlayerData.Data
    public PlayerData.Data Load(string fileName)
    {
        string unparsed = FSOperations.ReadFile(Path.Join(path, fileName));
        if (unparsed != string.Empty)
        {
            return JsonUtility.FromJson<PlayerData.Data>(unparsed);
        }
        return new PlayerData.Data();
    }
}
