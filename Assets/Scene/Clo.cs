using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clo : MonoBehaviour
{
    public Color col1, col2;
    public Color col3;
    public int a;
    public Vector3 hsv;
    void Start()
    {
        Debug.Log(col1);
        GetComponent<MeshRenderer>().material.SetVector("_offset", new Vector4(0, .6f, 0, 0));

      
    }

    // Update is called once per frame
    void Update()
    {
       // col3 = Color.HSVToRGB(hsv.x,hsv.y,hsv.z);
    }
}
