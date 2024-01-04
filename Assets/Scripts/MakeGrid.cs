#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

using UnityEditor;
using Unity.Collections.LowLevel.Unsafe;
using System;


//sphere .2 ,-0.017
//cube .16, 0

public class MakeGrid : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject tiles;
    public Vector2 layout;
    public ColorData data;
    public OffsetData OffsetData;

    private MeshRenderer _renderer;

    private List<Material> _materials;


    public Texture2D[] textures;
    public int TexIndex;
    public float offset;
    void Start()
    {
     //   Object o = GetComponent<MeshRenderer>().material;
        //Invoke("StopCoroutine", 1f);

        // GenerateOffsetData();
        InstiantiateTiles();
      
        TexIndex = 0;
        
    }
   
   
    public  void InstiantiateTiles()
    {
       
        int color = 0;
        int value = 2;

        for (int i = 6; i >-6; i-=2)
        {
            for (int j = -5; j<6; j+=2)
            {
               
               var a= Instantiate(tiles, new Vector3(j, i-1,5), Quaternion.Euler(90, 90, -90));
              
               _renderer= a.GetComponent<MeshRenderer>();

                Material[] materials;
                materials = _renderer.materials;
              
               // _renderer.GetMaterials(_materials);
               
                materials[0].color= data.colors[color];
                if (materials[0].color== UnityEngine.Color.white)
                {
                    materials[0].SetTexture("_BaseMap", textures[TexIndex]);
                    TexIndex++;
                }
              
                materials[3].SetTextureOffset("_BaseMap", OffsetData.Offset[color] + Vector2.one * offset);
               
                // _renderer.material.SetColor("_BaseMap", data.colors[color]);
                //  _renderer.material.SetTextureOffset("_Texture2D", OffsetData.Offset[color]);
                color++;
                value *= 2;
             //   yield return new WaitForSeconds(.1f);
               // _materials.Clear();
            }
        }
        //yield return null;
    }
    public int GetIndex(int value)
    {
        int j = 0;
        for (int i = 2; i <= value; i *= 2)
        {
            if (i == value)
            {
                value = j;
                break;

            }
            j++;
        }
        return value;
    }
    public Vector2 CalculateOffset(int value)
    {
        Vector2 offset = new Vector2(1, 0);
       
        int j = 0;
        for (int i = 2; i <= value; i *= 2)
        {
            if (i == value)
            {
                offset.x = OffsetData.Offset[j].x;
                offset.y = OffsetData.Offset[j].y;
                break;
            }

            j++;
        }
        return offset;
       // return offset;
    }

    public void GenerateOffsetData()
    {
        OffsetData.Offset = new Vector2[36];
        float size = 1f / 6f;
        Vector2 offset;
        int index = 0;
        for(int i = 5; i > -1; i--)
        {
            for(int j = 0; j < 6; j++)
            {
                offset = new Vector2(j*size, i*size);
                OffsetData.Offset[index] = offset;
               // Debug.Log(offset);
                index++;
            }
            Debug.Log(0);
        }
    }
  
}
#endif