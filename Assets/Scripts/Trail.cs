using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Trail : MonoBehaviour
{
    private IObjectPool<Trail> _pool;
    [SerializeField]
    private TrailRenderer _trail;
    [SerializeField]
    private Gradient _gradient;
    private GradientColorKey[] _colorKey;
    private GradientAlphaKey[] _alphaKey;


    private void Awake()
    {
        _colorKey = new GradientColorKey[2];
        _alphaKey = new GradientAlphaKey[2];
     //   _colorKey[0]= new GradientColorKey(Color.red, 0.0f);
        _colorKey[1] = new GradientColorKey(Color.white, 1.0f);
      //  _alphaKey[0] = new GradientAlphaKey(1, .5f);
        _alphaKey[1] = new GradientAlphaKey(0, 1);
        _gradient.SetKeys(_colorKey, _alphaKey);
        _trail.colorGradient = _gradient;
    }
    public void SetPool(IObjectPool<Trail> pool)
    {
        _pool = pool;
    }
    public IEnumerator SetRelease()
    {
        //1st time release
        yield return null;
        _trail.enabled = false;
        _pool.Release(this);

    }
    public void Release()
    {
      
            transform.SetParent(null);
            _trail.enabled = false;
            _pool.Release(this);
       
       
    }
    public void Initialized(Color color,Transform target)
    {
        _trail.enabled = false;
        transform.SetParent(target);
        // y = -.5f;
        transform.localPosition = new Vector3(0, -.1f, 0);
        transform.rotation = Quaternion.identity;

        _colorKey[0] = new GradientColorKey(color, .0f);
       
        _alphaKey[0] = new GradientAlphaKey(1, 0.5f);
        _gradient.SetKeys(_colorKey, _alphaKey);
        _trail.colorGradient = _gradient;
        gameObject.SetActive(true);
       

    }
   
   public void EnableTrail()
    {
        _trail.Clear();
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        _trail.Clear();
        _trail.enabled=true;
    }
   
}
