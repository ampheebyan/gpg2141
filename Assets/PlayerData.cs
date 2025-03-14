using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public struct Data
    {
        public string LevelName;
        public Vector2 SavedPosition;
        public int CollectedObjects;
    }
    public static PlayerData Instance;
    private Data _savedData = new ()
    {
        LevelName = "Zone1", // default
        CollectedObjects = 0, // init
        SavedPosition = new (9999, 9999) // excess value that won't get hit in play, magic number
    };
    
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
    }

    private void Start()
    {
        LoadData();
    }

    public void UpdateLevelName(string levelName)
    {
        _savedData.LevelName = levelName;
    }

    public string GetLevelName()
    {
        return _savedData.LevelName;
    }

    public int GetCollectedObjects()
    {
        return _savedData.CollectedObjects;
    }

    public Vector2 GetSavedPosition()
    {
        return _savedData.SavedPosition;
    }

    public void UpdateCollectedObjects()
    {
        _savedData.CollectedObjects++;
    }

    public void UpdateSavedPosition(Vector2 position)
    {
        _savedData.SavedPosition = position;
    }

    public void ResetData()
    {
        _savedData = new ()
        {
            LevelName = "Zone1", // default
            CollectedObjects = new (), // init list, populate with data later
            SavedPosition = new (9999, 9999) // excess value that won't get hit in play, magic number
        };
    }
    
    public void LoadData()
    {
        if (JSONHandler.Instance != null)
        {
            Data temporary = JSONHandler.Instance.Load("PlayerData.json");
            _savedData.CollectedObjects = temporary.CollectedObjects;
            _savedData.SavedPosition = temporary.SavedPosition;
            if (string.IsNullOrEmpty(temporary.LevelName)) return;
            _savedData.LevelName = temporary.LevelName;
            CollectableMenuText.Instance.SetText();
        }
        else
        {
            Debug.LogError("JSONHandler is null?");
        }
    }

    public void SaveData()
    {
        if (JSONHandler.Instance != null)
        {
            JSONHandler.Instance.Save("PlayerData.json", _savedData, true);
        }
    }
}
