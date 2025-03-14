using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataLoader : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private string levelName;

    public static LevelDataLoader current;

    public void SavePosition()
    {
        PlayerData.Instance.UpdateSavedPosition(player.transform.position);
    }
    
    private void Start()
    {
        if (PlayerData.Instance == null)
        {
            Debug.LogError("PlayerData is null");
            return;
        }
        
        current = this;
        if (PlayerData.Instance.GetLevelName() != levelName)
        {
            PlayerData.Instance.UpdateLevelName(levelName);
        }

        if (PlayerData.Instance.GetSavedPosition() != new Vector2(9999, 9999))
        {
            if (PlayerData.Instance.GetSavedPosition() == Vector2.zero) return;
            player.transform.position = PlayerData.Instance.GetSavedPosition();
        }
    }
}
