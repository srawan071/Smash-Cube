using ProMaxUtils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _2X : MonoBehaviour
{
    public Material _material;
    private int _basemap;
    Vector2 _offset;
    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _basemap = Shader.PropertyToID("_BaseMap");
    }
    private void Update()
    {
        _offset.y -= Time.deltaTime;
        _material.SetTextureOffset(_basemap, _offset);
    }

}
