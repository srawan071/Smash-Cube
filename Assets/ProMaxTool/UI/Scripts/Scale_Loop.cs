using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMaxUtils
{
    public class Scale_Loop : MonoBehaviour
    {
        public AnimationCurve curve;
        public float speed, time;
        public Vector3 startPos, endPos;


        // Update is called once per frame
        void Update()
        {
            time += Time.unscaledDeltaTime * speed;
            transform.localScale = Vector3.Lerp(startPos, endPos, curve.Evaluate(time));
        }
      
    }
}