using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
    public float startScale, endscale, offset;
    public Vector3 temppos;
    public float zoffset;
    void Start()
    {
        temppos.x = Screen.width / 2;
        temppos.y = Screen.height / 2;
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
            transform.localScale = Vector3.one * endscale;
        }
        // temppos = Input.mousePosition-new Vector3(0,offset,0);

        transform.localPosition = Input.mousePosition - temppos;

    }
}
