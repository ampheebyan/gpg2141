using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // struct to save/load data
    public struct Data
    {
        public string LevelName;
        public Vector2 SavedPosition;
        public int CollectedObjects;
    }
    // singleton instance
    public static PlayerData Instance;
    // struct instance
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
        // load data
        LoadData();
    }

    public void UpdateLevelName(string levelName)
    {
        // update level name
        _savedData.LevelName = levelName;
    }

    public string GetLevelName()
    {
        // return level name
        return _savedData.LevelName;
    }

    public int GetCollectedObjects()
    {
        // return collected objects
        return _savedData.CollectedObjects;
    }

    public Vector2 GetSavedPosition()
    {
        // return saved position
        return _savedData.SavedPosition;
    }

    public void UpdateCollectedObjects()
    {
        // increment collected objects
        _savedData.CollectedObjects++;
    }

    public void UpdateSavedPosition(Vector2 position)
    {
        // update saved position
        _savedData.SavedPosition = position;
    }

    // reset data
    public void ResetData()
    {
        _savedData = new ()
        {
            LevelName = "Zone1", // default
            CollectedObjects = new (), // init list, populate with data later
            SavedPosition = new (9999, 9999) // excess value that won't get hit in play, magic number
        };
    }
    
    // load from JSON and populate savedData
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

    // save to JSON
    public void SaveData()
    {
        if (JSONHandler.Instance != null)
        {
            JSONHandler.Instance.Save("PlayerData.json", _savedData, true);
        }
    }
}
