using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveResetUI : MonoBehaviour
{
    public void ResetButton()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.ResetData();
        }
    }
}
