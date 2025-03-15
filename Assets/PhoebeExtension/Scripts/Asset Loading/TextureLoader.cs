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
    // static texture keyvalue list
    public static List<KeyValuePair<string, Texture2D>> textures;
    
    // where to load from
    public enum Type
    {
        texture,
        sprite,
        bundle
    }

    // type in bundle/to load from
    public Type type = Type.texture;
    public Type typeInBundle = Type.texture;
        
    // name in bundle
    [SerializeField] private string nameInBundle = "texture";
    // filepath of bundle/asset
    [SerializeField] private string filePath = "";

    // should load on enable?
    [SerializeField] private bool loadOnEnable = false;
    
    private void OnEnable()
    {
        // check if loadonenable
        if (loadOnEnable)
        {
            // check what type
            switch (type)
            {
                // run through functions
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
        // validation
        if (this.type == Type.bundle)
        {
            if (path == "nothing") path = filePath;
            path = Path.Combine(Application.streamingAssetsPath, path);

            // check if file exists
            if (FSOperations.FileExists(path))
            {
                // load bundle
                AssetBundle bundle = AssetBundle.LoadFromFile(filePath);

                // ensure bundle actually loaded
                if (bundle == null)
                {
                    Debug.LogError("Bundle file is empty or failed to load.");
                    yield return null;
                }

                // load asset
                Texture2D asset = bundle.LoadAsset<Texture2D>(name);

                // figure out if sprite or texture and set texture slot accordingly
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
        // validation
        if (type == Type.sprite)
        {
            if (path == "nothing") path = filePath;
            path = Path.Combine(Application.streamingAssetsPath, path);

            // check if texture list is not null
            if (textures != null)
            {
                // find texture and set spriterender to found texture
                var _texture2D = textures.FirstOrDefault(_ => _.Key == path);

                if (_texture2D.Value != null)
                {
                    Sprite sprite = Sprite.Create(_texture2D.Value, new Rect(0, 0, _texture2D.Value.width, _texture2D.Value.height),
                        new Vector2(0.5f, 0f), _texture2D.Value.width > _texture2D.Value.height ? _texture2D.Value.width : _texture2D.Value.height);
                    if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                    {
                        spriteRenderer.sprite = sprite;
                    }
                    else
                    {
                        Debug.LogError("No renderer attached.");
                    }
                    // break so dont continue
                    yield break;
                }
            }
            else
            {
                // create if doesnt exist
                textures = new List<KeyValuePair<string, Texture2D>>();
            }
            
            // check if file exists
            if (FSOperations.FileExists(path))
            {
                // load raw data
                byte[] imageBytes = File.ReadAllBytes(path);

                // create texture
                Texture2D texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(imageBytes); // load into new texture from byte array

                // if textures.Count is more than 64, get rid of the 63 previous keyvaluepairs. leave to garbage collection.
                if (textures.Count > 64)
                {
                    textures = textures.Skip(63).ToList();
                }
                // add new texture as keyvaluepair
                textures.Add(new KeyValuePair<string, Texture2D>(path, texture2D));
                // create sprite and set spriterenderer sprite to new sprite
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
        // validation
        if (type == Type.texture)
        {
            if (path == "nothing") path = filePath;
            path = Path.Combine(Application.streamingAssetsPath, path);

            // check if textures is not null
            if (textures != null)
            {
                // find texture2D in list
                var _texture2D = textures.FirstOrDefault(_ => _.Key == path);

                // if found
                if (_texture2D.Value != null)
                {
                    // set texture
                    if (TryGetComponent<Renderer>(out Renderer _renderer))
                    {
                        _renderer.material.mainTexture = _texture2D.Value;
                    }
                    else
                    {
                        if (TryGetComponent<RawImage>(out RawImage image))
                        {
                            image.texture = _texture2D.Value;
                        }
                        else
                        {
                            Debug.LogError("No renderer(s) attached.");
                        }
                    }

                    yield break;
                }
            }
            else
            {
                // create if doesnt exist
                textures = new List<KeyValuePair<string, Texture2D>>();
            }
            
            // check if file exists
            if (FSOperations.FileExists(path))
            {
                // load raw data
                byte[] imageBytes = File.ReadAllBytes(path);
            
                // create temporary image
                Texture2D texture2D = new Texture2D(2, 2);
                // load raw data into image
                texture2D.LoadImage(imageBytes);
                
                // if more than 64 saved textures, clear 63 of them
                if (textures.Count > 64)
                {
                    textures = textures.Skip(63).ToList();
                }
                // add texture as keyvaluepair
                textures.Add(new KeyValuePair<string, Texture2D>(path, texture2D));
                // set texture to renderer component
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
