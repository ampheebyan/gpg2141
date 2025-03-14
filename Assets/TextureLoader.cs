using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextureLoader : MonoBehaviour
{
    public static List<KeyValuePair<string, Texture2D>> textures;
    
    
    public enum Type
    {
        texture,
        sprite,
        bundle
    }
    
    public Type type = Type.texture;
    public Type typeInBundle = Type.texture;
        
    [SerializeField] private string nameInBundle = "texture";
    [SerializeField] private string filePath = "";

    [SerializeField] private bool loadOnEnable = false;
    
    private void OnEnable()
    {
        if (loadOnEnable)
        {
            switch (type)
            {
                case Type.texture:
                    StartCoroutine(LoadTexture(filePath));
                    break;
                case Type.sprite:
                    StartCoroutine(LoadSprite(filePath));
                    break;
                case Type.bundle:
                    StartCoroutine(LoadFromBundle(typeInBundle, nameInBundle, filePath));
                    break;
            }
        }
    }
    
    public void SetNewFilePath(string path)
    {
        if (FSOperations.FileExists(path))
        {
            filePath = path;
        }
        else
        {
            Debug.LogError("File does not exist.");
        }
    }

    public IEnumerator LoadFromBundle(Type type, string name, string path = "nothing")
    {
        if (this.type == Type.bundle)
        {
            if (path == "nothing") path = filePath;
            path = Path.Combine(Application.streamingAssetsPath, path);

            if (FSOperations.FileExists(path))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(filePath);

                if (bundle == null)
                {
                    Debug.LogError("Bundle file is empty or failed to load.");
                    yield return null;
                }

                Texture2D asset = bundle.LoadAsset<Texture2D>(name);

                switch (type)
                {
                    case Type.texture:
                        if (TryGetComponent<Renderer>(out Renderer _renderer))
                        {
                            _renderer.material.mainTexture = asset;
                        }
                        else
                        {
                            Debug.LogError("No renderer attached.");
                        }

                        break;
                    case Type.sprite:
                        Sprite sprite = Sprite.Create(asset, new Rect(0, 0, asset.width, asset.height),
                            new Vector2(0.5f, 0f), asset.width > asset.height ? asset.width : asset.height);

                        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                        {
                            spriteRenderer.sprite = sprite;
                        }
                        else
                        {
                            Debug.LogError("No renderer attached.");
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        yield return null;
    }
    
    public IEnumerator LoadSprite(string path = "nothing")
    {
        if (type == Type.sprite)
        {
            if (path == "nothing") path = filePath;
            path = Path.Combine(Application.streamingAssetsPath, path);

            if (TextureLoader.textures != null)
            {
                Texture2D _texture2D = textures.First(_ => _.Key == path).Value;
                Sprite sprite = Sprite.Create(_texture2D, new Rect(0, 0, _texture2D.width, _texture2D.height),
                    new Vector2(0.5f, 0f), _texture2D.width > _texture2D.height ? _texture2D.width : _texture2D.height);
                if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.sprite = sprite;
                }
                else
                {
                    Debug.LogError("No renderer attached.");
                }
                yield return null;
            }
            else
            {
                TextureLoader.textures = new List<KeyValuePair<string, Texture2D>>();
            }
            
            if (FSOperations.FileExists(path))
            {
                byte[] imageBytes = File.ReadAllBytes(path);

                Texture2D texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(imageBytes);
                textures.Add(new KeyValuePair<string, Texture2D>(path, texture2D));
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                    new Vector2(0.5f, 0f), texture2D.width > texture2D.height ? texture2D.width : texture2D.height);

                if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.sprite = sprite;
                }
                else
                {
                    Debug.LogError("No renderer attached.");
                }
            }
            else
            {
                Debug.LogError($"Path '{filePath}' does not exist or is folder.");
            }
        }

        yield return null;
    }
    public IEnumerator LoadTexture(string path = "nothing")
    {
        if (type == Type.texture)
        {
            if (path == "nothing") path = filePath;
            path = Path.Combine(Application.streamingAssetsPath, path);

            if (TextureLoader.textures != null)
            {
                Texture2D _texture2D = textures.First(_ => _.Key == path).Value;
                if (TryGetComponent<Renderer>(out Renderer _renderer))
                {
                    _renderer.material.mainTexture = _texture2D;
                }
                else
                {
                    if (TryGetComponent<RawImage>(out RawImage image))
                    {
                        image.texture = _texture2D;
                    }
                    else
                    {
                        Debug.LogError("No renderer(s) attached.");
                    }
                }

                yield return null;
            }
            else
            {
                TextureLoader.textures = new List<KeyValuePair<string, Texture2D>>();
            }
            
            if (FSOperations.FileExists(path))
            {
                byte[] imageBytes = File.ReadAllBytes(path);
            
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(imageBytes);
                textures.Add(new KeyValuePair<string, Texture2D>(path, texture2D));
                if (TryGetComponent<Renderer>(out Renderer _renderer))
                {
                    _renderer.material.mainTexture = texture2D;
                }
                else
                {
                    if (TryGetComponent<RawImage>(out RawImage image))
                    {
                        image.texture = texture2D;
                    }
                    else
                    {
                        Debug.LogError("No renderer(s) attached.");
                    }
                }
            }
            else
            {
                Debug.LogError($"Path '{filePath}' does not exist or is folder.");
            }
        }

        yield return null;
    }
}
