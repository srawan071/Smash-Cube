using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarCube : MonoBehaviour
{
    private Material _material;
    [SerializeField]
    private OffsetData _offsetData;
    private int _baseMap;
    [SerializeField]
    private float offset;
    [SerializeField]
    private float _speed;
   
    [SerializeField]
    private Type _type;
    [SerializeField]
    private Image _fill;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private SpriteRenderer _Trophy;

    [SerializeField] ShowModel _model;

    private bool stop=false;

   private enum Type
    {
        Left,
        Middle,
        Right
    }
    private void Awake()
    {
        _baseMap = Shader.PropertyToID("_BaseMap");

        _material = GetComponent<MeshRenderer>().material;
        

    }
    public void UpdateSkin(int index)
    {
        switch (_type)
        {
            case Type.Left:
                _model.UpdateVisuals(index, -1);
                break;
            case Type.Middle:
                _model.UpdateVisuals(index, 0);
                break;
            case Type.Right:
                if (index >= 35)
                    return;
                    _model.UpdateVisuals(index, 1);
                break;

        }
       
    }
    public void UpdatePos(int index)
    {
      
            if (_type == Type.Left)
            {
                _type = Type.Right;
            if (index >= 35)
            {
                _material.color = _color;
                stop = true;
                StartCoroutine(MoveToRight(35, 125));
                return;
            }
         
            _model.UpdateVisuals(index, 1);
                _material.color = _color;
                StartCoroutine(MoveToRight(35, 125));
            }
            else if (_type == Type.Middle)
            {
                _type = Type.Left;
            
            _model.UpdateVisuals(index, -1);
            //middle

            _material.color = _color;
                StartCoroutine(Moveleft(35, -125));
            }
            else if (_type == Type.Right)
            {
                // _fill.fillAmount = 1;
                _type = Type.Middle;
          
            _model.UpdateVisuals(index, 0);
            _material.color = Color.white;
                StartCoroutine(Moveleft(50, 0));
            
        }
    }
    public IEnumerator Moveleft(int scale,int pos)
    {
        float desiredPos = pos;
        float tempPos =0;
        float tempScale = 0;
        float initialPos = transform.localPosition.x;
        float initialScale = transform.localScale.x;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime*_speed;
            tempPos = Mathf.Lerp(initialPos, desiredPos, t);
            tempScale = Mathf.Lerp(initialScale, scale, t);
            transform.localPosition = new Vector3(tempPos, transform.localPosition.y, transform.localPosition.z);
            transform.localScale = Vector3.one*tempScale;
           // _fill.fillAmount = -t*.5f+1;
          
            yield return null;
            
        }
        transform.localPosition = new Vector3(desiredPos, transform.localPosition.y, transform.localPosition.z);
        transform.localScale = Vector3.one * scale;
    }
    public IEnumerator MoveToRight(int scale,int pos)
    {
        float desiredPos = pos;
        float tempScale = 0;
       float desiredScale = scale;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime*_speed*2;
            tempScale = Mathf.Lerp(desiredScale, 0, t);
            transform.localScale = Vector3.one*tempScale;
          
            yield return null;
           
        }
        transform.localPosition = new Vector3(desiredPos, transform.localPosition.y, transform.localPosition.z);
        if (stop)
        {
            _Trophy.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime*_speed*2;
            tempScale = Mathf.Lerp(0, desiredScale, t);
            transform.localScale = Vector3.one * tempScale;
           
            yield return null;

        }
        transform.localScale = Vector3.one * desiredScale;
        transform.localPosition = new Vector3(desiredPos, transform.localPosition.y, transform.localPosition.z);
    }
}
