using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataLoader : MonoBehaviour
{
    // required in all levels
    
    // contains player reference and levelname
    [SerializeField] private GameObject player;
    [SerializeField] private string levelName;

    // instance that is the current instance
    public static LevelDataLoader current;

    public void SavePosition()
    {
        PlayerData.Instance.UpdateSavedPosition(player.transform.position); // save position to playerdata
    }
    
    private void Start()
    {
        if (PlayerData.Instance == null) // if this doesn't exist, why continue?
        {
            Debug.LogError("PlayerData is null");
            return;
        }
         
        // set current to this
        current = this;
        // if current level is not this, update
        if (PlayerData.Instance.GetLevelName() != levelName)
        {
            PlayerData.Instance.UpdateLevelName(levelName);
        }

        // if the saved position is not default or zero, load it
        if (PlayerData.Instance.GetSavedPosition() != new Vector2(9999, 9999))
        {
            if (PlayerData.Instance.GetSavedPosition() == Vector2.zero) return;
            player.transform.position = PlayerData.Instance.GetSavedPosition();
        }
    }
}
