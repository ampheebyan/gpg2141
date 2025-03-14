using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class JSONHandler : MonoBehaviour
{
    public static JSONHandler Instance;

    [SerializeField] private string path;
    
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
        if(string.IsNullOrEmpty(path)) path = Application.persistentDataPath;
    }

    public void Save(string fileName, object data, bool overwrite = false)
    {
        string json = JsonUtility.ToJson(data, true);
        bool outcome = FSOperations.WriteFile(Path.Join(path, fileName), json, overwrite);
        Debug.Log(outcome);
    }

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
