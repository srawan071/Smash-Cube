using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MyCanvas : MonoBehaviour
{

    private static RectTransform _rectTransform;
   
    public RectTransform _TargetRect;
   
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
      
    }
    private void Start()
    {
        RefreshPanel();
    }

    public void RefreshPanel()
    {

        _rectTransform = _rectTransform??GetComponent<RectTransform>();
        _rectTransform.sizeDelta = _TargetRect.sizeDelta;
        _rectTransform.localPosition = _TargetRect.localPosition;
        _rectTransform.localScale = _TargetRect.localScale;
    }

}

