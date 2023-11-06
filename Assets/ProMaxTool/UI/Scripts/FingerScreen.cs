using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerScreen : MonoBehaviour
{
    public float startScale, endscale,offset;
    public Vector3 temppos;
   public float zoffset;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.localScale = Vector3.one * startScale;
        }
        if (Input.GetMouseButtonUp(0))
        {
            transform.localScale = Vector3.one*endscale;
        }
       // temppos = Input.mousePosition-new Vector3(0,offset,0);
       
        transform.localPosition = Input.mousePosition-temppos;

    }
}
