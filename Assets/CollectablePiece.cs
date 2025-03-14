using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class CollectablePiece : MonoBehaviour
{
    [SerializeField] private string id = "unique id";
    [SerializeField] private TextureLoader textureLoader;
    [SerializeField] private AudioSource audioSource;

    private bool _lock = false;
    public string[] possibleTextureNames = new string[] { "chovy1.png", "chovy2.png", "chovy3.png" };
    private void Awake()
    {
        Random random = new Random();
        id = new (Enumerable.Range(1, 48).Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"[random.Next(62)]).ToArray());
        textureLoader.SetNewFilePath(Path.Combine(Application.streamingAssetsPath, possibleTextureNames[random.Next(possibleTextureNames.Length)]));
        textureLoader.LoadSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_lock == true) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (PlayerData.Instance)
            {
                PlayerData.Instance.UpdateCollectedObjects();
            }
            audioSource.Play();
            TryGetComponent(out SpriteRenderer spriteRenderer);
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
            _lock = true;
            Destroy(gameObject, 5);
        }
    }
}
