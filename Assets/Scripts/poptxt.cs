using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;
using System.Collections.Specialized;
using System.Text;
using ProMaxUtils;
using Unity.Burst.Intrinsics;

public class poptxt : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;
   // public int value;
    private IObjectPool<poptxt> _pool;
    private StringBuilder _sb= new StringBuilder(3);
    [SerializeField]
    private Pop_Up _popUp;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(45, 0, 0);
        // transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.5f);
        //  text = GetComponent<TextMeshPro>();
        //  text.text = "+" + value;
      
    }

   
    public void SetPool(IObjectPool<poptxt> pool)
    {
        _pool = pool;
    }
    public IEnumerator SetRelease()
    {
        
        yield return null;
      
        _pool.Release(this);

    }

    private void ReturnToPool()
    {
        _sb.Clear();
        _pool.Release(this);
    }
    public void Initialized(int value, Vector3 pos)
    {
        pos.z -= 2.5f;
        transform.position = pos;
        //  _sb.Clear();
        // _sb.Append(value);
        _sb.Clear();
        _sb.Append("+");
        _sb.Append(value);
        text.SetText(_sb.ToString());
       // text.SetText( value.ToString());
        gameObject.SetActive(true);
        Invoke("ReturnToPool", 1.2f);
        _popUp.OnButtonClick();
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(Vector3.up*5*Time.deltaTime);
     // transform.localPosition+= (Vector3.up*5 * Time.deltaTime);
    }
}
