using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMaxUtils
{
    public class Rotate_loop : MonoBehaviour
    {
        public Vector3 startRot, endRot;
        public float speed, time;
        public AnimationCurve curve;

        // Update is called once per frame
        void Update()
        {
            time += Time.unscaledDeltaTime * speed;
            transform.eulerAngles = Vector3.Lerp(startRot, endRot, curve.Evaluate(time));
        }
    }
}