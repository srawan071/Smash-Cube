using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMaxUtils { 
public class Slide_Loop : MonoBehaviour
{
    public Vector3 startPos, endPos;
    public float speed, time;
    public AnimationCurve curve;

    // Update is called once per frame
    void Update()
    {
        time += Time.unscaledDeltaTime * speed;
        transform.localPosition = Vector3.Lerp(startPos, endPos, curve.Evaluate(time));
    }
}
}
