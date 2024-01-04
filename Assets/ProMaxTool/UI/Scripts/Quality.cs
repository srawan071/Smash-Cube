using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quality : MonoBehaviour
{
    public static int fps=30;
   
    public static int ResFactor=1;
    public static event Action OnFpsChange;
    public static event Action OnResolutionChange;

    static Vector2 OriginalResolution;
    public static int QualityLevel;
   
    void Start()
    {
        
        Application.targetFrameRate = fps;
        OriginalResolution = new Vector2(Screen.width, Screen.height);
      //  ResolutionButton();
         SetAbsoluteResolution(720);
        
      // ResDebug();
        QualityLevel = PlayerPrefs.GetInt("QUALITY_LEVEL",2);

        QualitySettings.SetQualityLevel(QualityLevel);

    }
    public void FPSButton()
    {
        fps += 15;
        fps = (fps >= 100) ? 15 : fps;

        Application.targetFrameRate = fps;
        OnFpsChange?.Invoke();
      
    }
    public void ResolutionButton()
    {
        ResFactor++;
        ResFactor = (ResFactor > 5) ?  1 : ResFactor;
        Vector2 orgRes = OriginalResolution;
        orgRes = new Vector2(orgRes.x / Mathf.Sqrt(ResFactor), orgRes.y / Mathf.Sqrt(ResFactor));
        Screen.SetResolution((int)orgRes.x, (int)orgRes.y, true);
        OnResolutionChange?.Invoke();
     

    }
    public void SetAbsoluteResolution(int value)
    {
        float factor = OriginalResolution.y / OriginalResolution.x;
       // Debug.Log(factor + "Factor");
        Vector2 tempRes = new Vector2(value, value * factor);
      //  Debug.Log(tempRes);
        Screen.SetResolution((int)tempRes.x, (int)tempRes.y,true);
     //   Debug.Log(Screen.dpi+"DPI");
    }


    public static void ToggleQuality()
    {

        QualityLevel++;
        if (QualityLevel > 2)
            QualityLevel = 0;
        QualitySettings.SetQualityLevel(QualityLevel);
        PlayerPrefs.SetInt("QUALITY_LEVEL", QualityLevel);

       // ResDebug();

    }

    public static void ResDebug()
    {
        OriginalResolution = new Vector2(Screen.width, Screen.height);
        Debug.Log(OriginalResolution + "OriginalRes");

        Debug.Log(Screen.currentResolution + "currentres");
        Debug.Log(Display.main.systemWidth +"X" +Display.main.systemHeight + "NativeResolution");
        Debug.Log(Display.main.renderingWidth +"X"+ Display.main.renderingHeight + "RenderingResolution");
    }
}
