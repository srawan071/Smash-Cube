using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savetex : MonoBehaviour
{
    [SerializeField]
    RenderTexture rt;

    [SerializeField]
    string fileName;

    private void Start()
    {
        Invoke("SavePNG", 5f);
    }
    public void SavePNG()
    {
        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        var path = Application.dataPath+ "/" + fileName + ".png";
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        Debug.Log("Saved file to: " + path);
    }
}
