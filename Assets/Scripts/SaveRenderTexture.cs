using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveRenderTexture 
{
    
   
    public static void Save(this RenderTexture renderTexture, string file)
    {
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        UnityEngine.Object.Destroy(texture);

        System.IO.File.WriteAllBytes(file, bytes);

    }
}
