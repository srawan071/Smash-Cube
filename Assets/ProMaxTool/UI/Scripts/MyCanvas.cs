using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MyCanvas : MonoBehaviour
{

    private  RectTransform _rectTransform;
   
    public RectTransform _TargetRect;
   
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
      
    }
   
    IEnumerator Start()
    {
        yield return null;
        RefreshPanel();

    }

    public void RefreshPanel()
    {

        _rectTransform = _rectTransform??GetComponent<RectTransform>();
        _rectTransform.sizeDelta = _TargetRect.sizeDelta;
        _rectTransform.localPosition = _TargetRect.localPosition;
        _rectTransform.localScale = _TargetRect.localScale;
    }
    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            RefreshPanel();
       
    }
}

