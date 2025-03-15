using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLoad_Trigger : MonoBehaviour
{
    [SerializeField] private TextureLoader[] loaders;

    private void Update()
    {
        // if L is pressed just run through triggering all possible loaders
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var loader in loaders)
            {
                StartCoroutine(loader.LoadTexture());
            }
        } 
    }
}
