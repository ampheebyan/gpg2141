using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class CollectablePiece : MonoBehaviour
{
    // unique id: obsolete, but could be reused later
    [SerializeField] private string id = "unique id";
    // assetloaders
    [SerializeField] private TextureLoader textureLoader;
    [SerializeField] private AudioSource audioSource;
    // internal
    private bool _lock = false;
    public string[] possibleTextureNames = new string[] { "chovy1.png", "chovy2.png", "chovy3.png" };
    private void Awake()
    {
        // generate random ID
        Random random = new Random();
        id = new (Enumerable.Range(1, 48).Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"[random.Next(62)]).ToArray());
        // load sprite
        textureLoader.SetNewFilePath(Path.Combine(Application.streamingAssetsPath, possibleTextureNames[random.Next(possibleTextureNames.Length)]));
        textureLoader.LoadSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // lock to run once
        if (_lock == true) return;
        // check if player touched
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // update collected object number
            if (PlayerData.Instance)
            {
                PlayerData.Instance.UpdateCollectedObjects();
            }
            // play audio
            audioSource.Play();
            // disable spriterenderer
            TryGetComponent(out SpriteRenderer spriteRenderer);
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
            // lock and set destroy timer
            _lock = true;
            Destroy(gameObject, 5);
        }
    }
}
