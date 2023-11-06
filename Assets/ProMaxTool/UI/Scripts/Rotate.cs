using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 startRot, endRot;
    public float speed, time;
    public AnimationCurve curve;

    // Update is called once per frame
    void Update()
    {
        time += Time.unscaledDeltaTime * speed;
        transform.eulerAngles = Vector3.LerpUnclamped(startRot, endRot, time);
    }
}
