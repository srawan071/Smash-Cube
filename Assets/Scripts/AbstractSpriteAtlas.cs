using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class AbstractSpriteAtlas : MonoBehaviour
{
    [SerializeField]
    private SpriteAtlas _spriteAtlas;
    protected Image _imageContainer;
    [SerializeField]
    protected string _defaultValue;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Mode _type;
    

       enum Mode
    {
        Image,
        SpriteRenderer
    }
   protected virtual void Start()
    {
     //   InitializeImage();
       // Debug.Log("vstart");
    }

   protected void SetSpriteAtlasToImage(SpriteAtlas spriteAtlas,string spriteName)
    {
        _imageContainer.sprite = spriteAtlas.GetSprite(spriteName);
    }

    protected void SetSpriteAtlasToSpriteRenderer(SpriteAtlas spriteAtlas, string spriteName)
    {
        _spriteRenderer.sprite = spriteAtlas.GetSprite(spriteName);
    }
    protected virtual void OnValidate()
    {
        switch (_type)
        {

            case Mode.Image:
                InitializeImage();
                if (_spriteAtlas != null && _imageContainer != null)
                {
                    SetSpriteAtlasToImage(_spriteAtlas, _defaultValue);
                }
                break;
            case Mode.SpriteRenderer:
                InitializeSpriteRenderer();
                if (_spriteAtlas != null && _spriteRenderer != null)
                {
                    SetSpriteAtlasToSpriteRenderer(_spriteAtlas, _defaultValue);
                }
                break;

        }
      
       
    }
    private void InitializeSpriteRenderer()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
    }
    private void InitializeImage()
    {
        if (_imageContainer == null)
        {
            _imageContainer = this.gameObject.GetComponent<Image>();
        }
    }
}
