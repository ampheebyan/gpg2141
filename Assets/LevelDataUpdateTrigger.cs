using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataUpdateTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
        }
    }
}
