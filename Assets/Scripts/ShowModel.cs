using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModel : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private MeshFilter _meshFilter;
      private Material _material;
   [SerializeField] private Mesh _cube, _sphere;
   [SerializeField] private ShopData _shopData;
   [SerializeField] private OffsetData _offsetData;
   [SerializeField] private SkinData _skinData;
    [SerializeField] private MeshRenderer _meshRenderer;
    private float _offset = 0.003f;
  
    private int _baseMap;
    private void Awake()
    {
        _material = _meshRenderer.material;
        _baseMap = Shader.PropertyToID("_BaseMap");
        
    }

    public void UpdateVisuals(int index,int add)
    {
        if (_shopData.SkinSide == 0)
        {
            _meshFilter.mesh = _cube;
           
            _material.SetTexture(_baseMap, _skinData.NormalSkin[_shopData.ActiveSkin]);
            
        }
        else
        {
            _meshFilter.mesh = _sphere;

            _material.SetTexture(_baseMap, _skinData.EpicSkin[_shopData.ActiveSkin]);
           
        }

        _material.SetTextureOffset(_baseMap, _offsetData.Offset[index+add] + Vector2.one * _offset);
       
      
    }
    
}
