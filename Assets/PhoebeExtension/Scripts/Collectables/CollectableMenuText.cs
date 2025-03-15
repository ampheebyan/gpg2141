using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectableMenuText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public static CollectableMenuText Instance;
    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetText()
    {
        // set collectable text from playerdata
        text.SetText("Collectables collected: "+PlayerData.Instance.GetCollectedObjects());
    }
}
