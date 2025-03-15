using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gamekit2D
{
    public class StartUI : MonoBehaviour {

        public void Quit()
        {
            // modification to StartUI.cs: if there is a current LevelDataLoader instance, save position and then if PlayerData has an instance, save data.
            if (LevelDataLoader.current)
            {
                LevelDataLoader.current.SavePosition();
                if (PlayerData.Instance)
                {
                    PlayerData.Instance.SaveData();
                }
            }
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
		Application.Quit();
    #endif
        }
    }
}
