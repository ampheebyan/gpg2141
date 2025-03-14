using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLoad_Trigger : MonoBehaviour
{
    [SerializeField] private TextureLoader[] loaders;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var VARIABLE in loaders)
            {
                StartCoroutine(VARIABLE.LoadTexture());
            }
        } 
    }
}
